using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using IdentityProvider.Domain.Models;
using IdentityProvider.Infrastructure.Services.Abstract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Oidc.Domain.Models;
using Shared.Application.Configuration.Models;
using Shared.Authentication.Extensions;
using Utconnect.Common.Http.Uri;
using Utconnect.Common.Services.Abstractions;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityProvider.Areas.Identity.Pages.Account;

public class LoginModel(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IOidcService oidcService,
    IDateTime dateTime,
    IStringLocalizer<I18NResource> localizer,
    IOptions<HomeConfig> homeConfig,
    ILogger<LoginModel> logger
) : PageModel
{
    [BindProperty]
    public InputModel? Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

    public const string InvalidLoginAttempt = "InvalidLoginAttempt";

    public class InputModel
    {
        [Required]
        public string? Email { get; init; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; init; }

        public bool RememberMe { get; init; }
    }

    public async Task<IActionResult> OnGetAsync(string? returnUrl = null)
    {
        returnUrl ??= homeConfig.Value.Url + homeConfig.Value.TokenPath;

        if (User.Identity is { IsAuthenticated: true })
        {
            User? user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, localizer[InvalidLoginAttempt]);
                return Page();
            }

            ExchangeTokenResponse? token = await oidcService.Exchange(user, new CancellationToken());

            if (token != null)
            {
                return GetReturnUrlWithToken(returnUrl, token);
            }

            ModelState.AddModelError(string.Empty, localizer[InvalidLoginAttempt]);
            return Page();
        }

        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        ReturnUrl = returnUrl;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(
        string? returnUrl = null,
        CancellationToken cancellationToken = default)
    {
        returnUrl ??= homeConfig.Value.Url + homeConfig.Value.TokenPath;

        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (!ModelState.IsValid || Input?.Email == null || Input.Password == null)
        {
            return Page();
        }

        // This doesn't count login failures towards account lockout
        // To enable password failures to trigger account lockout, set lockoutOnFailure: true
        SignInResult result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe,
            lockoutOnFailure: false);
        if (result.Succeeded)
        {
            User? user = await userManager.FindByNameAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, localizer[InvalidLoginAttempt]);
                return Page();
            }

            Claim[] claims =
            [
                ..user.CreateClaims(dateTime.Now),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            ];

            List<ClaimsIdentity> identities = [new(claims, CookieAuthenticationDefaults.AuthenticationScheme)];
            ClaimsPrincipal principal = new(identities);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            ExchangeTokenResponse? token = await oidcService.Exchange(user, cancellationToken);

            if (token != null)
            {
                return GetReturnUrlWithToken(returnUrl, token);
            }

            logger.LogError("Cannot get token");
            ModelState.AddModelError(string.Empty, localizer[InvalidLoginAttempt]);
            return Page();
        }

        if (result.RequiresTwoFactor)
        {
            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
        }

        if (result.IsLockedOut)
        {
            logger.LogWarning("User account locked out");
            return RedirectToPage("./Lockout");
        }

        ModelState.AddModelError(string.Empty, localizer[InvalidLoginAttempt]);
        return Page();
    }

    private RedirectResult GetReturnUrlWithToken(string returnUrl, ExchangeTokenResponse token)
    {
        Uri returnUrlWithToken = new Uri(returnUrl)
            .AddParameter("access_token", token.AccessToken)
            .AddParameter("token_type", token.TokenType)
            .AddParameter("expires_in", token.ExpiresIn.ToString())
            .AddParameter("refresh_token", token.RefreshToken);

        return Redirect(returnUrlWithToken.ToString());
    }
}
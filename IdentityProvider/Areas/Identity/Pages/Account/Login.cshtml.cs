using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using IdentityProvider.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Shared.Application.Localization;
using Shared.UtconnectIdentity.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace IdentityProvider.Areas.Identity.Pages.Account;

public class LoginModel(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IIdentityService identityService,
    ILogger<LoginModel> logger,
    IStringLocalizer<I18NResource> localizer)
    : PageModel
{
    [BindProperty]
    public InputModel? Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; } = new List<AuthenticationScheme>();

    public string? ReturnUrl { get; set; }

    [TempData]
    public string? ErrorMessage { get; set; }

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
        returnUrl ??= Url.Content("~/");

        if (User.Identity is { IsAuthenticated: true })
        {
            return Redirect(returnUrl);
        }

        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        // Clear the existing external cookie to ensure a clean login process
        // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

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
                ModelState.AddModelError(string.Empty, localizer["InvalidLoginAttempt"]);
                return Page();
            }

            (string scheme, ClaimsPrincipal principal, AuthenticationProperties properties) =
                identityService.GetNewClaims(user);
            await HttpContext.SignInAsync(scheme, principal, properties);

            return Redirect(returnUrl);
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

        ModelState.AddModelError(string.Empty, localizer["InvalidLoginAttempt"]);
        return Page();
    }
}
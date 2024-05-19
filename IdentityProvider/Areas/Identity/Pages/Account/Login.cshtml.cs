using System.ComponentModel.DataAnnotations;
using IdentityProvider.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.UtconnectIdentity.Services;

namespace IdentityProvider.Areas.Identity.Pages.Account;

public class LoginModel(
    SignInManager<User> signInManager,
    UserManager<User> userManager,
    IIdentityService identityService,
    ILogger<LoginModel> logger)
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

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; init; }
    }

    public async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        // await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
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
        var result =
            await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe,
                lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = await userManager.FindByNameAsync(Input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return Page();
            }

            var (scheme, principal, properties) = identityService.GetNewClaims(user);
            await HttpContext.SignInAsync(scheme, principal, properties);

            logger.LogInformation("User logged in");
            return LocalRedirect(returnUrl);
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

        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        return Page();
    }
}
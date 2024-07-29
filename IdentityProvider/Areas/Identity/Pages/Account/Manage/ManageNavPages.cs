using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentityProvider.Areas.Identity.Pages.Account.Manage;

public static class ManageNavPages
{
    public static readonly List<ManageNavPageItem> Items = [
        new ManageNavPageItem("Profile", "", "file-user"),
        new ManageNavPageItem("Email", "email", "email-address"),
        new ManageNavPageItem("Password", "change-password", "lock")
    ];
    
    public static string Index => "Index";

    public static string Email => "Email";

    public static string ChangePassword => "ChangePassword";

    public static bool IsActivePageNav(ViewContext viewContext, string page)
    {
        string? activePage = viewContext.ViewData["ActivePage"] as string
                             ?? Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
        return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase);
    }

    public record ManageNavPageItem(string Name, string Url, string Icon);
}
namespace Shared.UtconnectIdentity.Models;

internal class ClaimUser(
    Guid identifier,
    string userName,
    string name,
    bool isAuthenticated,
    List<int> permissions,
    List<int> roles)
    : IUser
{
    public Guid Id { get; set; } = identifier;
    public string? UserName { get; set; } = userName;
    public string? Email { get; set; }
    public string Name { get; set; } = name;
    public bool IsAuthenticated { get; set; } = isAuthenticated;
    public List<int> Permissions { get; set; } = permissions;
    public List<int> Roles { get; set; } = roles;
}
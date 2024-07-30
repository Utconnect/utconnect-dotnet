namespace Shared.UtconnectIdentity.Models;

public interface IUser
{
    Guid Id { get; set; }
    string? UserName { get; set; }
    string? Email { get; set; }
    string Name { get; set; }
    bool IsAuthenticated { get; set; }
    List<int> Permissions { get; }
    List<int> Roles { get; }
}
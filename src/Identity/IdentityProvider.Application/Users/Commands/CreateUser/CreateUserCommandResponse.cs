namespace IdentityProvider.Application.Users.Commands.CreateUser;

public class CreateUserCommandResponse
{
    public string UserName { get; set; } = default!;
    public Guid Id { get; set; }
}
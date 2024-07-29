namespace Shared.Infrastructure.Email.Services.Abstract;

public interface IEmailService
{
    Task<bool> SendResetPassword(string email, string callbackUrl, CancellationToken cancellationToken);
}
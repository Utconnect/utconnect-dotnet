using Microsoft.Extensions.Options;
using RestSharp;
using Shared.Infrastructure.Email.Configs;
using Shared.Infrastructure.Email.Models;
using Shared.Infrastructure.Email.Services.Abstract;

namespace Shared.Infrastructure.Email.Services.Implementations;

internal class EmailService(IOptions<EmailConfig> emailConfig) : IEmailService
{
    private const string ResetPassword = "RESET_PASSWORD";
    private const string CreateTeacher = "CREATE_TEACHER";

    public async Task<bool> SendResetPassword(string email, string callbackUrl, CancellationToken cancellationToken)
    {
        Dictionary<string, string> placeholders = new() { { "callbackUrl", callbackUrl } };
        return await SendAsync(email, ResetPassword, placeholders, cancellationToken);
    }

    public async Task<bool> SendCreateTeacher(string email, string name, string userName,
        CancellationToken cancellationToken)
    {
        Dictionary<string, string> placeholders = new() { { "name", name }, { "userName", userName } };
        return await SendAsync(email, CreateTeacher, placeholders, cancellationToken);
    }

    private async Task<bool> SendAsync(
        string email,
        string templateCode,
        Dictionary<string, string> placeholders,
        CancellationToken cancellationToken)
    {
        RestClientOptions options = new(emailConfig.Value.Url);
        RestClient client = new(options);

        RestRequest request = new(emailConfig.Value.SendPath);
        SendEmailModel sendEmailModel = new(email, templateCode, placeholders);
        request.AddJsonBody(sendEmailModel);

        RestResponse response = await client.ExecutePostAsync(request, cancellationToken);
        return response.IsSuccessful;
    }
}
namespace Shared.Infrastructure.Email.Models;

public sealed record SendEmailModel(string To, string TemplateCode, Dictionary<string, string> Placeholders);
using JetBrains.Annotations;

namespace Shared.Authentication.Models;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GeneratedToken
{
    public string Token { get; set; } = default!;
    public DateTime Expiration { get; set; }
}
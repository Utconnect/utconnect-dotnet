﻿namespace Shared.Authentication.Configurations;

public class JwtConfig
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpirationMinutes { get; set; }
}
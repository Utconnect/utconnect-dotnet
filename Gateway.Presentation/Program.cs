WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
// builder.Services.AddAuthorizationBuilder()
//     .AddPolicy("customPolicy", policy =>
//         policy.RequireAuthenticatedUser());

WebApplication app = builder.Build();

// app.UseAuthentication();
// app.UseAuthorization();

app.MapReverseProxy();

app.Run();
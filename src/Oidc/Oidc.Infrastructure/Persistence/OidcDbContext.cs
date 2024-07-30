using Microsoft.EntityFrameworkCore;

namespace Oidc.Infrastructure.Persistence;

public class OidcDbContext(DbContextOptions<OidcDbContext> options) : DbContext(options);
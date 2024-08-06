using Microsoft.EntityFrameworkCore;

namespace Friday.Infrastructure.Persistence;

public class FridayDbContext(DbContextOptions<FridayDbContext> options) : DbContext(options);
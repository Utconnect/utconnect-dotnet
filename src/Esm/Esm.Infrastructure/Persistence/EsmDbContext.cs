using System.Reflection;
using Esm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Db.Interceptors;

namespace Esm.Infrastructure.Persistence;

public class EsmDbContext(
    DbContextOptions<EsmDbContext> options,
    AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
    : DbContext(options), IEsmDbContext
{
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<DepartmentShiftGroup> DepartmentShiftGroups => Set<DepartmentShiftGroup>();
    public DbSet<Examination> Examinations => Set<Examination>();
    public DbSet<ExaminationData> ExaminationData => Set<ExaminationData>();
    public DbSet<ExaminationEvent> ExaminationEvents => Set<ExaminationEvent>();
    public DbSet<FacultyShiftGroup> FacultyShiftGroups => Set<FacultyShiftGroup>();
    public DbSet<InvigilatorShift> InvigilatorShift => Set<InvigilatorShift>();
    public DbSet<Shift> Shifts => Set<Shift>();
    public DbSet<ShiftGroup> ShiftGroups => Set<ShiftGroup>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditableEntitySaveChangesInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
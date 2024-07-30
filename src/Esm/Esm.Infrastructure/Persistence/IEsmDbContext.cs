using Esm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Esm.Infrastructure.Persistence;

public interface IEsmDbContext
{
    DbSet<Candidate> Candidates { get; }
    DbSet<DepartmentShiftGroup> DepartmentShiftGroups { get; }
    DbSet<Examination> Examinations { get; }
    DbSet<ExaminationData> ExaminationData { get; }
    DbSet<ExaminationEvent> ExaminationEvents { get; }
    DbSet<FacultyShiftGroup> FacultyShiftGroups { get; }
    DbSet<InvigilatorShift> InvigilatorShift { get; }
    DbSet<Shift> Shifts { get; }
    DbSet<ShiftGroup> ShiftGroups { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());

    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
}
using Utconnect.Common.Infrastructure.Db.Entities;

namespace Esm.Domain.Entities;

public class FacultyShiftGroup : BaseAuditableEntity<int>
{
    public int InvigilatorsCount { get; set; }
    public int CalculatedInvigilatorsCount { get; set; }
    public Guid FacultyId { get; set; }

    public Guid ShiftGroupId { get; set; }
    public ShiftGroup ShiftGroup { get; set; } = null!;

    public ICollection<DepartmentShiftGroup> DepartmentShiftGroups { get; set; } = new List<DepartmentShiftGroup>();
}
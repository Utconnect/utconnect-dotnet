using ESM.Domain.Enums;
using Utconnect.Common.Infrastructure.Db.Entities;

namespace Esm.Domain.Entities;

public class ShiftGroup : BaseEntity<int>
{
    public ExamMethod Method { get; set; }
    public int InvigilatorsCount { get; set; }
    public int RoomsCount { get; set; }
    public DateTime StartAt { get; set; }
    public int? Shift { get; set; }
    public bool IsDepartmentAssign { get; set; }
    public Guid ModuleId { get; set; }

    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;

    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    public ICollection<FacultyShiftGroup> FacultyShiftGroups { get; set; } = new List<FacultyShiftGroup>();
}
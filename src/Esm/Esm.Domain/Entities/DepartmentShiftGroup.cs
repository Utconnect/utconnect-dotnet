using System.ComponentModel.DataAnnotations;
using Shared.Domain;

namespace Esm.Domain.Entities;

public class DepartmentShiftGroup : BaseAuditableEntity<int>
{
    public Guid? DepartmentId { get; set; }
    public Guid? UserId { get; set; }

    [MaxLength(100)]
    public string? TemporaryInvigilatorName { get; set; }

    public Guid FacultyShiftGroupId { get; set; }
    public FacultyShiftGroup FacultyShiftGroup { get; set; } = null!;
}
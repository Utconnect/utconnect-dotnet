using Utconnect.Common.Infrastructure.Db.Entities;

namespace Esm.Domain.Entities;

public class InvigilatorShift : BaseAuditableEntity<int>, ISoftDelete
{
    public int OrderIndex { get; set; }
    public int Paid { get; set; }
    public Guid? UserId { get; set; }

    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public Guid ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;
}
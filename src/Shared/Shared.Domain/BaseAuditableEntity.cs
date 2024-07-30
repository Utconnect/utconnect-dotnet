namespace Shared.Domain;

public abstract class BaseAuditableEntity : BaseAuditableEntity<Guid>;

public abstract class BaseAuditableEntity<T> : BaseEntity<T>
{
    public DateTime CreatedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public Guid? LastModifiedBy { get; set; }
}
namespace Shared.Domain;

public interface ISoftDelete
{
    DateTime? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }
}
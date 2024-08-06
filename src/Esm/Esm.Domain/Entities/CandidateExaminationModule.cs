using Utconnect.Common.Infrastructure.Db.Entities;

namespace Esm.Domain.Entities;

public class CandidateExaminationModule : BaseAuditableEntity<int>, ISoftDelete
{
    public Guid ModuleId { get; set; }

    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;

    public Guid CandidateId { get; set; }
    public Candidate Candidate { get; set; } = null!;
}
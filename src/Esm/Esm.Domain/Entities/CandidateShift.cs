using Shared.Domain;

namespace Esm.Domain.Entities;

public class CandidateShift : BaseEntity<int>
{
    public int OrderIndex { get; set; }

    public Guid CandidateId { get; set; }
    public Candidate Candidate { get; set; } = null!;

    public Guid ShiftId { get; set; }
    public Shift Shift { get; set; } = null!;
}
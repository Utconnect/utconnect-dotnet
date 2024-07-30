using System.ComponentModel.DataAnnotations;
using Shared.Domain;

namespace Esm.Domain.Entities;

public class Candidate : BaseAuditableEntity
{
    [MaxLength(20)]
    public string DisplayId { get; set; } = null!;

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    public bool IsStudent { get; set; }

    public ICollection<CandidateShift> CandidateShift { get; set; } = new List<CandidateShift>();

    public ICollection<CandidateExaminationModule> ExaminationModules { get; set; } =
        new List<CandidateExaminationModule>();
}
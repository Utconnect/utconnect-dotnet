using System.ComponentModel.DataAnnotations;
using ESM.Domain.Enums;
using Shared.Domain;

namespace Esm.Domain.Entities;

public class Examination : BaseAuditableEntity
{
    [MaxLength(20)]
    public string? DisplayId { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = null!;

    [MaxLength(200)]
    public string? Description { get; set; }

    public DateTime? ExpectStartAt { get; set; }

    public DateTime? ExpectEndAt { get; set; }

    public ExaminationStatus Status { get; set; }

    public ICollection<ShiftGroup> ShiftGroups { get; set; } = new List<ShiftGroup>();

    public ICollection<CandidateExaminationModule> CandidatesOfModule { get; set; } =
        new List<CandidateExaminationModule>();

    public ICollection<ExaminationEvent> Events { get; set; } = new List<ExaminationEvent>();
}
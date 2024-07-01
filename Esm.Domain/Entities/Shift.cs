using System.ComponentModel.DataAnnotations;
using Shared.Domain;

namespace Esm.Domain.Entities;

public class Shift : BaseEntity
{
    public int ExamsCount { get; set; }
    public int CandidatesCount { get; set; }
    public int InvigilatorsCount { get; set; }
    public Guid? RoomId { get; set; }

    [MaxLength(10000)]
    public string? Report { get; set; }

    public Guid? HandedOverUserId { get; set; }

    public Guid ShiftGroupId { get; set; }
    public ShiftGroup ShiftGroup { get; set; } = null!;


    public ICollection<InvigilatorShift> InvigilatorShift { get; set; } = new List<InvigilatorShift>();

    public ICollection<CandidateShift> CandidateShift { get; set; } = new List<CandidateShift>();
}
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Domain.Enums;
using Shared.Domain;

namespace Esm.Domain.Entities;

public class ExaminationEvent : BaseEntity<int>
{
    public ExaminationStatus Status { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreateAt { get; set; }

    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;
}
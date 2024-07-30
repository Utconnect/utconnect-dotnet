using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ESM.Domain.Enums;
using Shared.Domain;

namespace Esm.Domain.Entities;

public class ExaminationData : BaseEntity<int>
{
    [MaxLength(20)]
    public string? ModuleId { get; set; }

    [MaxLength(100)]
    public string? ModuleName { get; set; }

    [MaxLength(200)]
    public string? ModuleClass { get; set; }

    public int? Credit { get; set; }

    public ExamMethod? Method { get; set; }

    public DateTime? Date { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public int? Shift { get; set; }

    public int? CandidatesCount { get; set; }

    public int? RoomsCount { get; set; }

    [MaxLength(50)]
    public string? Rooms { get; set; }

    [MaxLength(100)]
    public string? Faculty { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }

    public bool? DepartmentAssign { get; set; }

    public Guid ExaminationId { get; set; }
    public Examination Examination { get; set; } = null!;

    [NotMapped]
    public Dictionary<string, ExaminationDataError> Errors { get; set; } = new();

    [NotMapped]
    public Dictionary<string, List<KeyValuePair<string, string>>> Suggestions { get; set; } = new();
}

public class ExaminationDataError(string message)
{
    public string Message { get; set; } = message;
}

public class ExaminationDataError<T> : ExaminationDataError
{
    public List<T>? Data { get; set; }

    public ExaminationDataError(string message, List<T>? data = null) : base(message)
    {
        Message = message;
        Data = data;
    }
}
namespace ESM.Domain.Enums;

public enum ExamMethod
{
    Select = 0,
    Write = 1,
    Practice = 2,
    Oral = 3,
    Report1 = 4,
    Report2 = 5
}

public static class ExamMethodHelper
{
    private static readonly Dictionary<string, ExamMethod> Mapping = new()
    {
        { "trắc nghiệm", ExamMethod.Select },
        { "tự luận", ExamMethod.Write },
        { "thực hành", ExamMethod.Practice },
        { "vấn đáp", ExamMethod.Oral },
        { "báo cáo 1", ExamMethod.Report1 },
        { "báo cáo 2", ExamMethod.Report2 }
    };

    public static ExamMethod? FromString(string method) => Mapping.GetValueOrDefault(method);
}
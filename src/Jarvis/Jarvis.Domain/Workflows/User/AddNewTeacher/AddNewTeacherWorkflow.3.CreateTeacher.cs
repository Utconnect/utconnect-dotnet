using System.Net.Mime;
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using Microsoft.AspNetCore.Http;
using Utconnect.Common.Models;

namespace Jarvis.Domain.Workflows.User.AddNewTeacher;

public partial class AddNewTeacherWorkflow
{
    private SendHttpRequest CreateTeacher(Variable varCreateUserResponse, Variable varCreateTeacherResponse)
    {
        Uri serverAddress = new(teacherConfig.Value.Url);
        Uri createTeacherApiUrl = new(serverAddress, "teacher");

        return new SendHttpRequest
        {
            Id = "3.CreateTeacher",
            Name = "Create teacher",
            Metadata = { { "displayText", "Create teacher" } },
            Url = new Input<Uri?>(createTeacherApiUrl),
            Method = new Input<string>(HttpMethods.Post),
            ContentType = new Input<string?>(MediaTypeNames.Application.Json),
            Content = new Input<object?>(ctx => new CreateTeacherRequest
            {
                UserId = varCreateUserResponse.Get<CreateUserResponse>(ctx)!.Data.Id,
                DepartmentId = ctx.GetInput<string>("DepartmentId"),
                FacultyId = ctx.GetInput<string>("FacultyId")
            }),
            ParsedContent = new Output<object?>(varCreateTeacherResponse)
        };
    }

    private sealed class CreateTeacherRequest
    {
        public Guid UserId { get; set; }
        public string? DepartmentId { get; set; }
        public string? FacultyId { get; set; }
    }

    private sealed class CreateTeacherResponse : Result<Guid>;
}
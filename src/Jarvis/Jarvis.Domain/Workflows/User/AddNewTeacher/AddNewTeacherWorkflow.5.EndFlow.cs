using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows.Models;
using Microsoft.AspNetCore.Http;

namespace Jarvis.Domain.Workflows.User.AddNewTeacher;

public partial class AddNewTeacherWorkflow
{
    private SendHttpRequest EndFlow()
    {
        Uri serverAddress = new(teacherConfig.Value.Url);

        return new SendHttpRequest
        {
            Id = "5.EndFlow",
            Name = "Send finish flow notification",
            Metadata = { { "displayText", "Send finish flow notification" } },
            Url = new Input<Uri?>(ctx =>
            {
                Guid teacherId = ctx.GetVariable<CreateTeacherResponse>(VariableCreateTeacherResponse)!.Data;
                return new Uri(serverAddress, $"teacher/{teacherId}/finish");
            }),
            Method = new Input<string>(HttpMethods.Put)
        };
    }
}
using System.Net.Mime;
using Elsa.Http;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using IdentityProvider.Domain.Constants;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Jarvis.Domain.Workflows.User.AddNewTeacher;

public partial class AddNewTeacherWorkflow
{
    private SendHttpRequest GrantRole(Variable varCreateUserResponse)
    {
        Uri serverAddress = new(identityConfig.Value.Url);

        return new SendHttpRequest
        {
            Id = "2.GrantRole",
            Name = "Add user to role",
            Metadata = { { "displayText", "Add user to role" } },
            Url = new Input<Uri?>(ctx =>
                new Uri(serverAddress,
                    $"api/user/{varCreateUserResponse.Get<CreateUserResponse>(ctx)!.Data.Id}/role")),
            Method = new Input<string>(HttpMethods.Patch),
            ContentType = new Input<string?>(MediaTypeNames.Application.Json),
            Content = new Input<object?>(
                JsonConvert.SerializeObject(new GrantRoleRequest { Roles = [RoleConstant.Teacher] })
            )
        };
    }

    private sealed class GrantRoleRequest
    {
        public List<string> Roles { get; set; } = [];
    }
}
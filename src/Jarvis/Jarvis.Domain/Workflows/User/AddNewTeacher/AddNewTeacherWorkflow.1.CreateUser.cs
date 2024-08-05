using System.Net.Mime;
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Shared.Presentation.Models;

namespace Jarvis.Domain.Workflows.User.AddNewTeacher;

public partial class AddNewTeacherWorkflow
{
    private SendHttpRequest CreateUser(Variable varCreateUserResponse)
    {
        Uri serverAddress = new(identityConfig.Value.Url);
        Uri createUserApiUrl = new(serverAddress, "api/user");

        return new SendHttpRequest
        {
            Id = "1.CreateUser",
            Name = "Create user",
            Metadata = { { "displayText", "Create user" } },
            Url = new Input<Uri?>(createUserApiUrl),
            Method = new Input<string>(HttpMethods.Post),
            ContentType = new Input<string?>(MediaTypeNames.Application.Json),
            Content = new Input<object?>(ctx =>
            {
                CreateUserRequest request = new() { Name = ctx.GetInput<string>("Name")! };
                return JsonConvert.SerializeObject(request);
            }),
            ParsedContent = new Output<object?>(varCreateUserResponse)
        };
    }

    private sealed class CreateUserRequest
    {
        public string Name { get; set; } = default!;
    }

    private sealed class CreateUserResponse : Result<CreateUserResponseData>;

    private sealed class CreateUserResponseData
    {
        public string UserName { get; set; } = default!;
        public Guid Id { get; set; } = Guid.Empty;
    }
}
using Elsa.Extensions;
using Elsa.Http;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Activities.Flowchart.Activities;
using Elsa.Workflows.Activities.Flowchart.Models;
using Elsa.Workflows.Contracts;
using Elsa.Workflows.Memory;
using Elsa.Workflows.Models;
using Microsoft.Extensions.Options;
using Shared.Application.Configuration.Models;

namespace Jarvis.Domain.Workflows.User.AddNewTeacher;

public partial class AddNewTeacherWorkflow(
    IOptions<IdentityConfig> identityConfig,
    IOptions<TeacherConfig> teacherConfig) : WorkflowBase
{
    public const string DefinitionId = "User_AddNewTeacher";

    protected override void Build(IWorkflowBuilder builder)
    {
        InputDefinition inputName = builder.WithInput<string>("Name");
        InputDefinition inputDepartmentId = builder.WithInput<string?>("DepartmentId");
        InputDefinition inputFacultyId = builder.WithInput<string?>("FacultyId");
        InputDefinition inputEmail = builder.WithInput<string?>("Email");

        Variable varCreateUserResponse = builder.WithVariable<CreateUserResponse>(VariableCreateUserResponse, default!)
            .WithWorkflowStorage();
        Variable varCreateTeacherResponse =
            builder.WithVariable<CreateTeacherResponse>(VariableCreateTeacherResponse, default!).WithWorkflowStorage();

        Start start = new();
        SendHttpRequest createUser = CreateUser(varCreateUserResponse);
        SendHttpRequest grantRole = GrantRole(varCreateUserResponse);
        SendHttpRequest createTeacher = CreateTeacher(varCreateUserResponse, varCreateTeacherResponse);
        If sendEmailToNewUser = SendEmailToNewUser();
        SendHttpRequest endFlow = EndFlow();

        builder.Inputs = [inputName, inputDepartmentId, inputFacultyId, inputEmail];
        builder.DefinitionId = DefinitionId;
        builder.Name = "[User] Add new teacher";
        builder.Root = new Flowchart
        {
            Id = "AddNewTeacher",
            Name = "Add new teacher",
            Variables = { varCreateUserResponse, varCreateTeacherResponse },
            Activities =
            {
                start,
                createUser,
                grantRole,
                createTeacher,
                sendEmailToNewUser,
                endFlow
            },
            Connections =
            {
                new Connection(start, createUser),
                new Connection(createUser, grantRole),
                new Connection(grantRole, createTeacher),
                new Connection(createTeacher, sendEmailToNewUser),
                new Connection(createTeacher, endFlow)
            }
        };
    }
}
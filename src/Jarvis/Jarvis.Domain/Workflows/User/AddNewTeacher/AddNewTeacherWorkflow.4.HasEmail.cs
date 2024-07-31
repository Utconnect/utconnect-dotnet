using Elsa.Extensions;
using Elsa.Workflows;
using Elsa.Workflows.Activities;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using Shared.Infrastructure.Email.Services.Abstract;

namespace Jarvis.Domain.Workflows.User.AddNewTeacher;

public partial class AddNewTeacherWorkflow
{
    private static If SendEmailToNewUser()
    {
        return new If
        {
            Id = "4.HasEmail",
            Name = "Has email",
            Metadata = { { "displayText", "Has email" } },
            Condition = new Input<bool>(ctx => !string.IsNullOrEmpty(ctx.GetInput<string>("Email"))),
            Then = new SendEmailActivity()
        };
    }

    [Activity("Email")]
    private class SendEmailActivity : CodeActivity
    {
        public SendEmailActivity()
        {
            Name = "Send email to new user";
        }

        protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
        {
            IEmailService emailService = context.GetRequiredService<IEmailService>();
            var email = context.GetWorkflowInput<string>("Email");
            var name = context.GetWorkflowInput<string>("Name");
            var userName = context.GetWorkflowInput<string>("UserName");

            await emailService.SendCreateTeacher(email, name, userName, CancellationToken.None);
        }
    }
}
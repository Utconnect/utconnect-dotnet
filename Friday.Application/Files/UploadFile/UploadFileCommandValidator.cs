using FluentValidation;

namespace Friday.Application.Files.UploadFile;

public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
    public UploadFileCommandValidator()
    {
        RuleFor(e => e.Name).NotEmpty();
        RuleFor(e => e.Email).EmailAddress();
    }
}
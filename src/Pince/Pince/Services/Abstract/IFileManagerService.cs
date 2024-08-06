namespace Pince.Services.Abstract;

public interface IFileManagerService
{
    Task<string?> Upload(CancellationToken cancellationToken);
}
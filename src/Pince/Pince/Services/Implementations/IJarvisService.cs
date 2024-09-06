using Pince.Services.Abstract;

namespace Pince.Services.Implementations;

internal class FileManagerService : IFileManagerService
{
    public Task<string?> Upload(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
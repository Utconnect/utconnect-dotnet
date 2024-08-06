using Microsoft.Extensions.Options;
using Pince.Models;
using Pince.Services.Abstract;

namespace Pince.Services.Implementations;

internal class FileManagerService(IOptions<FileManagerConfig> config) : IFileManagerService
{
    public Task<string?> Upload(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
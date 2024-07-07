using Shared.Services.Abstractions;

namespace Shared.Services.Implementations;

internal class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
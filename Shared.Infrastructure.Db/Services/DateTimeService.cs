namespace Shared.Infrastructure.Db.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
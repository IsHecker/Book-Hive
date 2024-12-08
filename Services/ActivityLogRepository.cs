using BookHive.Models;

namespace BookHive.Services;

public class ActivityLogRepository
{
    private readonly AppDbContext _context;

    public ActivityLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task LogActivity(ActivityLog activityLog)
    {
        await _context.ActivityLogs.AddAsync(activityLog);
        await _context.SaveChangesAsync();
    }

    public IEnumerable<ActivityLog> GetUserActivityLogs(int userId)
    {
        return _context.ActivityLogs.Where(a => a.UserId == userId);
    }
}
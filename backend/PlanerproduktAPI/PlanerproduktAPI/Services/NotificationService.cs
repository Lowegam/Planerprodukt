using Microsoft.EntityFrameworkCore;
using PlanerproduktAPI.Data;
using PlanerproduktAPI.Models;

namespace PlanerproduktAPI.Services;

public class NotificationService : BackgroundService
{
    private readonly IServiceProvider _provider;

    public NotificationService(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _provider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var tasks = await db.Tasks
                    .Where(t => t.Status != TaskStatusEnum.Done && t.Deadline != null)
                    .ToListAsync();

                foreach (var task in tasks)
                {
                    if (task.Deadline.HasValue && task.Deadline.Value <= DateTime.UtcNow.AddHours(24))
                    {
                        var exists = await db.Notifications
                            .AnyAsync(n => n.UserId == task.UserId && n.Message.Contains(task.Title));
                        if (!exists)
                        {
                            db.Notifications.Add(new Notification
                            {
                                UserId = task.UserId,
                                Message = $"Zadanie '{task.Title}' ma termin za mniej niz 24h!"
                            });
                        }
                    }
                }
                await db.SaveChangesAsync();
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using PlanerproduktAPI.Data;
using PlanerproduktAPI.Models;

namespace PlanerproduktAPI.Tests;

public class DatabaseTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=test.db")
            .Options;
        var db = new AppDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public async Task CanAddUser()
    {
        var db = GetDbContext();
        var user = new User { UserName = "testuser", Email = "test@test.pl" };
        db.Users.Add(user);
        await db.SaveChangesAsync();
        var saved = await db.Users.FirstOrDefaultAsync(u => u.UserName == "testuser");
        Assert.NotNull(saved);
    }

    [Fact]
    public async Task CanAddTask()
    {
        var db = GetDbContext();
        var user = new User { UserName = "taskuser", Email = "task@test.pl" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var task = new TaskItem { Title = "Test", UserId = user.Id };
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
        Assert.True(task.Id > 0);
    }
}
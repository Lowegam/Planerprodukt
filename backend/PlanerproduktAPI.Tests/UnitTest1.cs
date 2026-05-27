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
    public async Task Can_Add_User_To_Database()
    {
        var db = GetDbContext();
        var user = new User { UserName = "test", Email = "test@test.pl" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var saved = await db.Users.FirstOrDefaultAsync(u => u.UserName == "test");
        Assert.NotNull(saved);
        Assert.Equal("test@test.pl", saved.Email);
    }

    [Fact]
    public async Task Can_Add_Task_To_Database()
    {
        var db = GetDbContext();

        var user = new User { UserName = "taskuser", Email = "task@test.pl" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var task = new TaskItem
        {
            Title = "Testowe zadanie",
            UserId = user.Id
        };
        db.Tasks.Add(task);
        await db.SaveChangesAsync();

        var saved = await db.Tasks.FirstOrDefaultAsync(t => t.Title == "Testowe zadanie");
        Assert.NotNull(saved);
        Assert.Equal(user.Id, saved.UserId);
    }
}
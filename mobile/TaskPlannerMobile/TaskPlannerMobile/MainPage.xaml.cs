#nullable disable
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TaskPlannerMobile;

public partial class MainPage : ContentPage
{
    private static readonly HttpClient client = new() { BaseAddress = new Uri("http://localhost:5056/") };
    private List<TaskItem> _allTasks = new();
    private bool _showTodayOnly;

    public MainPage()
    {
        InitializeComponent();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginPage.Token);
        Loaded += async (s, e) => await LoadTasks();
    }

    private async Task LoadTasks()
    {
        try
        {
            var resp = await client.GetStringAsync("api/Tasks");
            _allTasks = JsonConvert.DeserializeObject<List<TaskItem>>(resp) ?? new();
            UpdateList();
        }
        catch { LblStatus.Text = "Błąd ładowania"; }
    }

    private void UpdateList()
    {
        var tasks = _showTodayOnly ? _allTasks.Where(t => t.deadline?.Date == DateTime.Today).ToList() : _allTasks;
        CvTasks.ItemsSource = tasks;
        LblStatus.Text = $"Zadań: {tasks.Count}";
        LblWelcome.Text = _showTodayOnly ? "Zadania na dziś" : "Wszystkie zadania";
    }

    private async void BtnAdd_Clicked(object? sender, EventArgs e)
    {
        var title = await DisplayPromptAsync("Nowe zadanie", "Tytuł:");
        if (string.IsNullOrWhiteSpace(title)) return;
        var json = JsonConvert.SerializeObject(new { title, description = "", status = "New", priority = "Medium" });
        await client.PostAsync("api/Tasks", new StringContent(json, Encoding.UTF8, "application/json"));
        await LoadTasks();
    }

    private async void BtnRefresh_Clicked(object? sender, EventArgs e) => await LoadTasks();

    private void BtnToday_Clicked(object? sender, EventArgs e)
    {
        _showTodayOnly = !_showTodayOnly;
        UpdateList();
    }

    private async void SwipeDone(object? sender, EventArgs e)
    {
        if (sender is SwipeItem si && si.BindingContext is TaskItem task)
        {
            var json = JsonConvert.SerializeObject(new { title = task.title, description = task.description, status = "Done", priority = task.priority });
            await client.PutAsync($"api/Tasks/{task.id}", new StringContent(json, Encoding.UTF8, "application/json"));
            await LoadTasks();
        }
    }

    private async void SwipeDelete(object? sender, EventArgs e)
    {
        if (sender is SwipeItem si && si.BindingContext is TaskItem task)
        {
            await client.DeleteAsync($"api/Tasks/{task.id}");
            await LoadTasks();
        }
    }
}

public class TaskItem
{
    public int id { get; set; }
    public string title { get; set; } = "";
    public string description { get; set; } = "";
    public string status { get; set; } = "";
    public string priority { get; set; } = "";
    public DateTime? deadline { get; set; }
}
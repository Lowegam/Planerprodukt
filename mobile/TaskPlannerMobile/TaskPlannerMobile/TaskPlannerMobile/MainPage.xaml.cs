#nullable disable
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TaskPlannerMobile;

public partial class MainPage : ContentPage
{
    private static readonly HttpClient client = new() { BaseAddress = new Uri("http://localhost:5056/") };
    private List<TaskItem> _all = new();
    private bool _today;

    public MainPage()
    {
        InitializeComponent();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginPage.Token);
        Loaded += async (s, e) => await Load();
    }

    private async Task Load()
    {
        var resp = await client.GetStringAsync("api/Tasks");
        _all = JsonConvert.DeserializeObject<List<TaskItem>>(resp) ?? new();
        UpdateList();
    }

    private void UpdateList()
    {
        var tasks = _today ? _all.Where(t => t.deadline?.Date == DateTime.Today).ToList() : _all;
        CvTasks.ItemsSource = tasks;
        LblTitle.Text = _today ? "Zadania na dziś" : "Wszystkie zadania";
    }

    private async void BtnAdd_Clicked(object sender, EventArgs e)
    {
        var title = await DisplayPromptAsync("Nowe", "Tytuł:");
        if (string.IsNullOrWhiteSpace(title)) return;
        var status = await DisplayActionSheet("Status", "Anuluj", null, "New", "InProgress", "Done");
        if (status == "Anuluj" || status == null) return;
        var priority = await DisplayActionSheet("Priorytet", "Anuluj", null, "Low", "Medium", "High");
        if (priority == "Anuluj" || priority == null) return;
        var json = JsonConvert.SerializeObject(new { title, description = "", status, priority });
        await client.PostAsync("api/Tasks", new StringContent(json, Encoding.UTF8, "application/json"));
        await Load();
    }

    private async void BtnRefresh_Clicked(object sender, EventArgs e) => await Load();
    private void BtnToday_Clicked(object sender, EventArgs e) { _today = !_today; UpdateList(); }

    private async void SwipeDone(object sender, EventArgs e)
    {
        if (sender is SwipeItem si && si.BindingContext is TaskItem t)
        {
            var json = JsonConvert.SerializeObject(new { t.title, t.description, status = "Done", t.priority });
            await client.PutAsync($"api/Tasks/{t.id}", new StringContent(json, Encoding.UTF8, "application/json"));
            await Load();
        }
    }

    private async void SwipeDelete(object sender, EventArgs e)
    {
        if (sender is SwipeItem si && si.BindingContext is TaskItem t)
        {
            await client.DeleteAsync($"api/Tasks/{t.id}");
            await Load();
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
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TaskPlannerMobile;

public partial class MainPage : ContentPage
{
    private static readonly HttpClient client = new() { BaseAddress = new Uri("http://10.0.2.2:5056/") }; // Android emulator
    // Dla Windows: "http://localhost:5056/"

    public MainPage()
    {
        InitializeComponent();
        Loaded += async (s, e) => await Login();
    }

    private async Task Login()
    {
        try
        {
            var json = JsonConvert.SerializeObject(new { username = "test", password = "Test123!" });
            var resp = await client.PostAsync("api/Auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
            var result = JsonConvert.DeserializeObject<dynamic>(await resp.Content.ReadAsStringAsync());
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)result.token);
            await LoadTasks();
        }
        catch (Exception ex) { LblInfo.Text = "Błąd: " + ex.Message; }
    }

    private async Task LoadTasks()
    {
        try
        {
            var resp = await client.GetStringAsync("api/Tasks");
            var tasks = JsonConvert.DeserializeObject<List<TaskItem>>(resp);
            CvTasks.ItemsSource = tasks;
            LblInfo.Text = $"Zadań: {tasks?.Count ?? 0}";
        }
        catch { LblInfo.Text = "Błąd ładowania"; }
    }

    private async void BtnAdd_Clicked(object sender, EventArgs e)
    {
        var title = TxtTitle.Text;
        if (string.IsNullOrWhiteSpace(title)) return;
        var json = JsonConvert.SerializeObject(new { title, description = "", status = "New", priority = "Medium" });
        await client.PostAsync("api/Tasks", new StringContent(json, Encoding.UTF8, "application/json"));
        TxtTitle.Text = "";
        await LoadTasks();
    }

    private async void BtnRefresh_Clicked(object sender, EventArgs e) => await LoadTasks();

    private async void SwipeDelete_Invoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.BindingContext is TaskItem task)
        {
            await client.DeleteAsync($"api/Tasks/{task.id}");
            await LoadTasks();
        }
    }

    private async void SwipeDone_Invoked(object sender, EventArgs e)
    {
        if (sender is SwipeItem item && item.BindingContext is TaskItem task)
        {
            var json = JsonConvert.SerializeObject(new { title = task.title, description = task.description, status = "Done", priority = task.priority });
            await client.PutAsync($"api/Tasks/{task.id}", new StringContent(json, Encoding.UTF8, "application/json"));
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
}
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using Newtonsoft.Json;
using System.IO;


namespace TaskPlannerDesktop;

public partial class MainWindow : Window
{
    private static readonly HttpClient client = new() { BaseAddress = new Uri("http://localhost:5056/") };

    public MainWindow() => InitializeComponent();

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var json = JsonConvert.SerializeObject(new { username = "test", password = "Test123!" });
        var resp = await client.PostAsync("api/Auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
        var result = JsonConvert.DeserializeObject<dynamic>(await resp.Content.ReadAsStringAsync());
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)result.token);
        await LoadTasks();
    }

    private async Task LoadTasks()
    {
        var resp = await client.GetStringAsync("api/Tasks");
        DgTasks.ItemsSource = JsonConvert.DeserializeObject<List<TaskItem>>(resp);
    }

    private async void BtnAdd_Click(object sender, RoutedEventArgs e)
    {
        var window = new TaskWindow();
        if (window.ShowDialog() == true)
        {
            var json = JsonConvert.SerializeObject(new { title = window.TaskTitle, description = window.Description, status = window.Status, priority = window.Priority });
            await client.PostAsync("api/Tasks", new StringContent(json, Encoding.UTF8, "application/json"));
            await LoadTasks();
        }
    }
    private async void BtnDelete_Click(object sender, RoutedEventArgs e)
    {
        if (DgTasks.SelectedItem is not TaskItem t) return;
        await client.DeleteAsync($"api/Tasks/{t.id}");
        await LoadTasks();
    }

    private async void BtnRefresh_Click(object sender, RoutedEventArgs e) => await LoadTasks();

    private void BtnExport_Click(object sender, RoutedEventArgs e)
    {
        var sb = new StringBuilder();
        sb.AppendLine("Tytuł;Status;Priorytet");
        if (DgTasks.ItemsSource is List<TaskItem> tasks)
            foreach (var t in tasks) sb.AppendLine($"{t.title};{t.status};{t.priority}");
        File.WriteAllText("raport.csv", sb.ToString());
        MessageBox.Show("Zapisano!");
    }
}
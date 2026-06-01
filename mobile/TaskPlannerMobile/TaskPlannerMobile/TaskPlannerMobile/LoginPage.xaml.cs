#nullable disable
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TaskPlannerMobile;

public partial class LoginPage : ContentPage
{
    private static readonly HttpClient client = new() { BaseAddress = new Uri("http://localhost:5056/") };
    public static string Token;

    public LoginPage() => InitializeComponent();

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        Loading.IsVisible = true;
        try
        {
            var json = JsonConvert.SerializeObject(new { username = TxtUsername.Text, password = TxtPassword.Text });
            var resp = await client.PostAsync("api/Auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
            if (resp.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<dynamic>(await resp.Content.ReadAsStringAsync());
                Token = (string)result.token;
                Application.Current.MainPage = new NavigationPage(new MainPage());
            }
            else LblError.Text = "Błąd logowania";
        }
        catch { LblError.Text = "Błąd"; }
        Loading.IsVisible = false;
    }

    private async void BtnRegister_Clicked(object sender, EventArgs e)
    {
        var u = TxtUsername.Text; var p = TxtPassword.Text;
        if (string.IsNullOrWhiteSpace(u) || string.IsNullOrWhiteSpace(p)) { LblError.Text = "Uzupełnij pola"; return; }
        try
        {
            var json = JsonConvert.SerializeObject(new { username = u, email = u + "@example.com", password = p });
            var resp = await client.PostAsync("api/Auth/register", new StringContent(json, Encoding.UTF8, "application/json"));
            if (resp.IsSuccessStatusCode)
            {
                LblError.Text = "OK! Zaloguj się.";
                LblError.TextColor = Colors.Green;
            }
            else
            {
                var error = await resp.Content.ReadAsStringAsync();
                LblError.Text = error;
                LblError.TextColor = Colors.Red;
            }
        }
        catch (Exception ex) { LblError.Text = ex.Message; }
    }
}
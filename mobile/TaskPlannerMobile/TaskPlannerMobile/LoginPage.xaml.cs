#nullable disable
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace TaskPlannerMobile;

public partial class LoginPage : ContentPage
{
    private static readonly HttpClient client = new() { BaseAddress = new Uri("http://localhost:5056/") };
    public static string Token { get; private set; }

    public LoginPage()
    {
        InitializeComponent();
    }

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
            else
            {
                LblError.Text = "Nieprawidłowe dane";
            }
        }
        catch { LblError.Text = "Błąd połączenia"; }
        Loading.IsVisible = false;
    }

    private async void BtnRegister_Clicked(object sender, EventArgs e)
    {
        var username = TxtUsername.Text;
        var password = TxtPassword.Text;
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            LblError.Text = "Uzupełnij pola";
            LblError.TextColor = Colors.Red;
            return;
        }
        try
        {
            var email = username + "@example.com";
            var json = JsonConvert.SerializeObject(new { username, email, password });
            var resp = await client.PostAsync("api/Auth/register", new StringContent(json, Encoding.UTF8, "application/json"));
            if (resp.IsSuccessStatusCode)
            {
                LblError.Text = "Konto utworzone! Zaloguj się.";
                LblError.TextColor = Colors.Green;
            }
            else
            {
                LblError.Text = "Użytkownik już istnieje";
                LblError.TextColor = Colors.Red;
            }
        }
        catch { LblError.Text = "Błąd"; }
    }
}
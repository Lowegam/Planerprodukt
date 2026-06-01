using System.Windows;

namespace TaskPlannerDesktop;

public partial class TaskWindow : Window
{
    public string TaskTitle => TxtTitle.Text;
    public string Description => TxtDesc.Text;
    public string Status => ((System.Windows.Controls.ComboBoxItem)CmbStatus.SelectedItem)?.Tag?.ToString() ?? "New";
    public string Priority => ((System.Windows.Controls.ComboBoxItem)CmbPriority.SelectedItem)?.Tag?.ToString() ?? "Medium";

    public TaskWindow()
    {
        InitializeComponent();
        CmbStatus.SelectedIndex = 0;
        CmbPriority.SelectedIndex = 1;
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtTitle.Text)) { MessageBox.Show("Tytuł wymagany!"); return; }
        DialogResult = true;
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
using System.Windows;
using System.Windows.Controls;

namespace TaskPlannerDesktop;

public partial class TaskWindow : Window
{
    public string TaskTitle => TxtTitle.Text;
    public string Description => TxtDesc.Text;
    public string Status => ((ComboBoxItem)CmbStatus.SelectedItem).Tag?.ToString() ?? "New";
    public string Priority => ((ComboBoxItem)CmbPriority.SelectedItem).Tag?.ToString() ?? "Medium";

    public TaskWindow(TaskItem? task = null)
    {
        InitializeComponent();
        if (task != null)
        {
            TxtTitle.Text = task.title;
            TxtDesc.Text = task.description;
            CmbStatus.SelectedIndex = task.status switch { "New" => 0, "InProgress" => 1, "Done" => 2, _ => 0 };
            CmbPriority.SelectedIndex = task.priority switch { "Low" => 0, "Medium" => 1, "High" => 2, _ => 1 };
        }
    }

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtTitle.Text))
        {
            MessageBox.Show("Tytuł wymagany!");
            return;
        }
        DialogResult = true;
    }

    private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
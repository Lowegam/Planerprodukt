namespace TaskPlannerDesktop;

public class TaskItem
{
    public int id { get; set; }
    public string title { get; set; } = "";
    public string description { get; set; } = "";
    public string status { get; set; } = "";
    public string priority { get; set; } = "";
    public DateTime? deadline { get; set; }
}
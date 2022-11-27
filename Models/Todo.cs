using System.ComponentModel;

public class Todo
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public ICollection<Tags>? Tags { get; set; }
}

public class TodoItemDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public IEnumerable<string>? Tags { get; set; }
}

public enum Tags
{   
    [Description("home")] Home,
    [Description("work")] Work,
    [Description("health")] Health,
    [Description("school")] School,
    [Description("vacation")] Vacation,
    [Description("ideas")] Ideas,
    [Description("cooking")] Cooking,
    [Description("payment")] Payment,
    [Description("meeting")] Meeting,
}
namespace ThreadAlert.Entities;

public class Message
{
    public Guid Id { get; set; }
    public int DangerousObjectId {  get; set; }
    public DangerousObject DangerousObject { get; set; } = null!;
    public string Title { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public string Description { get; set; } = null!;
    public bool IsActived { get; set; }
    public string Location { get; set; } = null!;
    
    public Priority Priority { get; set; }
}

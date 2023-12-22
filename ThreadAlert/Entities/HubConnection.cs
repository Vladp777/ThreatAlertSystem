namespace ThreadAlert.Entities
{
    public class HubConnection
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; } = null!;

        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}

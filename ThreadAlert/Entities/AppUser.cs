using Microsoft.AspNetCore.Identity;

namespace ThreadAlert.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public List<Message> Messages { get; set; }

    public List<DangerousObject> DangerousObjects { get; set; }

}
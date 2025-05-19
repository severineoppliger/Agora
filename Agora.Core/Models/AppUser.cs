using Microsoft.AspNetCore.Identity;

namespace Agora.Core.Models;

public class AppUser : IdentityUser
{
    public int Credit { get; set; }
}
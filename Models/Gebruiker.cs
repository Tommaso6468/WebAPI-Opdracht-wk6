using Microsoft.AspNetCore.Identity;

public class Gebruiker : IdentityUser
{
    public Geslacht Geslacht { get; set; }
}
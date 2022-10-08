using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class RoleController
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<Gebruiker> _userManager;

    public RoleController(RoleManager<IdentityRole> roleManager, UserManager<Gebruiker> userManager)
    {
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("setup")]
    public void Setup()
    {
        SetupRoles(_roleManager, _userManager);
    }

    private async void SetupRoles(RoleManager<IdentityRole> roleManager, UserManager<Gebruiker> userManager)
    {
        string[] roles = { "Admin", "Medewerker", "Gast" };
        foreach (string r in roles)
        {
            if (!await roleManager.RoleExistsAsync(r))
            {
                await roleManager.CreateAsync(new IdentityRole(r));
            }
        }

        Gebruiker user = await userManager.FindByNameAsync("admin");
        if (user == null)
        {
            Gebruiker newUser = new Gebruiker { UserName = "admin" };
            await userManager.CreateAsync(newUser, "Admin1234!");
            user = newUser;
        }

        await userManager.AddToRoleAsync(user, "Admin");
    }
}
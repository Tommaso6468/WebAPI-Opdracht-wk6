using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Policy = "Gast", AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class LikeController : ControllerBase
{
    private PretparkContext context;
    private UserManager<Gebruiker> _userManager;

    public LikeController(PretparkContext pretparkContext, UserManager<Gebruiker> userManager)
    {
        context = pretparkContext;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<ActionResult<Like>> PostLike(string attractieNaam)
    {
        Gebruiker user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        if (user == null) return Problem("Gebruiker niet gevonden");

        if (await context.Likes.FirstOrDefaultAsync(l => l.gast.Id == user.Id && l.attractie.Naam == attractieNaam) != null)
        {
            return Problem("Attractie al geliked");
        }

        if (await context.Attracties.FirstOrDefaultAsync(a => a.Naam == attractieNaam) == null)
        {
            return Problem("Attractie niet gevonden");
        }

        var newLike = new Like { attractie = await context.Attracties.FirstOrDefaultAsync(a => a.Naam == attractieNaam), gast = user };

        await context.Likes.AddAsync(newLike);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetLike", new { id = newLike.Id }, newLike);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLike(string attractieNaam)
    {
        Gebruiker user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);

        if (user == null) return Problem("Gebruiker niet gevonden");

        if (context.Likes == null)
        {
            return NotFound();
        }
        var foundLike = await context.Likes.FirstOrDefaultAsync(l => l.attractie.Naam == attractieNaam && l.gast.Id == user.Id);
        if (foundLike == null)
        {
            return NotFound();
        }

        context.Likes.Remove(foundLike);
        await context.SaveChangesAsync();

        return Ok();
    }

}
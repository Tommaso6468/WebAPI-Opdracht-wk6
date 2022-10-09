using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AttractieController : ControllerBase
{
    private PretparkContext context;

    public AttractieController(PretparkContext pretparkContext)
    {
        context = pretparkContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attractie>>> GetAttracties()
    {
        if (context.Attracties == null)
        {
            return NotFound();
        }
        return await context.Attracties.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<(Attractie, int)>> GetAttractie(int id)
    {
        if (context.Attracties == null)
        {
            return NotFound();
        }
        var attractie = await context.Attracties.FirstOrDefaultAsync(a => a.Id == id);

        if (attractie == null)
        {
            return NotFound();
        }

        var likes = context.Likes.Select(a => a.Id == id).Count();

        return (attractie, likes);
    }

    [Authorize(Policy = "Admin", AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<ActionResult<Attractie>> PostAttractie(Attractie attractie)
    {
        if (context.Attracties == null)
        {
            return Problem("Entity set 'PretparkContext.Gebruiker'  is null.");
        }
        await context.Attracties.AddAsync(attractie);

        return CreatedAtAction("GetAttractie", new { id = attractie.Id }, attractie);
    }

    [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttractie(int id)
    {
        if (context.Attracties == null)
        {
            return NotFound();
        }
        var attractie = await context.Attracties.FirstOrDefaultAsync(a => a.Id == id);
        if (attractie == null)
        {
            return NotFound();
        }

        context.Attracties.Remove(attractie);

        return Ok();
    }

}
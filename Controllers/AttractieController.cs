using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AttractieController : ControllerBase
{
    private readonly PretparkContext _pretparkContext;

    public AttractieController(PretparkContext pretparkContext)
    {
        _pretparkContext = pretparkContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attractie>>> GetAttracties()
    {
        if (_pretparkContext.Attracties == null)
        {
            return NotFound();
        }
        return await _pretparkContext.Attracties.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<(Attractie, int)>> GetAttractie(int id)
    {
        if (_pretparkContext.Attracties == null)
        {
            return NotFound();
        }
        var attractie = await _pretparkContext.Attracties.FirstOrDefaultAsync(a => a.Id == id);

        if (attractie == null)
        {
            return NotFound();
        }

        var likes = _pretparkContext.Likes.Select(a => a.Id == id).Count();

        return (attractie, likes);
    }

    [Authorize(Policy = "Admin", AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<ActionResult<Attractie>> PostAttractie(Attractie attractie)
    {
        if (_pretparkContext.Attracties == null)
        {
            return Problem("Entity set 'PretparkContext.Gebruiker'  is null.");
        }
        await _pretparkContext.Attracties.AddAsync(attractie);
        await _pretparkContext.SaveChangesAsync();

        return CreatedAtAction("GetAttractie", new { id = attractie.Id }, attractie);
    }

    [Authorize(Policy = "Admin", AuthenticationSchemes = "Bearer")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttractie(int id)
    {
        if (_pretparkContext.Attracties == null)
        {
            return NotFound();
        }
        var attractie = await _pretparkContext.Attracties.FirstOrDefaultAsync(a => a.Id == id);
        if (attractie == null)
        {
            return NotFound();
        }

        _pretparkContext.Attracties.Remove(attractie);
        await _pretparkContext.SaveChangesAsync();

        return Ok();
    }

}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "Gast", AuthenticationSchemes = "Bearer")]
[ApiController]
[Route("api/[controller]")]
public class LikeController : ControllerBase
{
    private PretparkContext context;

    public LikeController(PretparkContext pretparkContext)
    {
        context = pretparkContext;
    }

    [HttpPost]
    public async Task<ActionResult<Like>> PostLike(Like like)
    {
        if (await context.Likes.FirstOrDefaultAsync(l => l.gast == like.gast && l.attractie == like.attractie) != null)
        {
            return Problem("Attractie al geliked");
        }

        await context.Likes.AddAsync(like);

        return CreatedAtAction("GetLike", new { id = like.Id }, like);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteLike(Like like)
    {
        if (context.Likes == null)
        {
            return NotFound();
        }
        var foundLike = await context.Likes.FirstOrDefaultAsync(l => l.Id == like.Id || (l.attractie == like.attractie && l.gast == like.gast));
        if (foundLike == null)
        {
            return NotFound();
        }

        context.Likes.Remove(foundLike);

        return Ok();
    }

}
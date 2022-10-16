using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Opdracht_wk6.Controllers
{
    [Authorize(Policy = "Admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase
    {

        private readonly UserManager<Gebruiker> _userManager;
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GebruikerController(UserManager<Gebruiker> userManager, SignInManager<Gebruiker> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // GET: api/Gebruiker
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gebruiker>>> GetGebruiker()
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }
            return await _userManager.Users.ToListAsync();
            // return await _context.Users.ToListAsync();
        }

        // GET: api/Gebruiker/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gebruiker>> GetGebruiker(string id)
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }
            var gebruiker = await _userManager.FindByIdAsync(id);

            if (gebruiker == null)
            {
                return NotFound();
            }

            return gebruiker;
        }

        // PUT: api/Gebruiker/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGebruiker(string id, Gebruiker identityUser)
        {
            if (id != identityUser.Id)
            {
                return BadRequest();
            }

            // _context.Entry(gebruiker).State = EntityState.Modified;
            

            try
            {
                await _userManager.CreateAsync(identityUser);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GebruikerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Gebruiker
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gebruiker>> PostGebruiker(Gebruiker identityUser)
        {
            if (_userManager.Users == null)
            {
                return Problem("Entity set 'PretparkContext.Gebruiker'  is null.");
            }
            await _userManager.CreateAsync(identityUser);

            return CreatedAtAction("GetGebruiker", new { id = identityUser.Id }, identityUser);
        }

        // DELETE: api/Gebruiker/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGebruiker(string id)
        {
            if (_userManager.Users == null)
            {
                return NotFound();
            }
            var gebruiker = await _userManager.FindByIdAsync(id);
            if (gebruiker == null)
            {
                return NotFound();
            }

            await _userManager.DeleteAsync(gebruiker);

            return NoContent();
        }

        private bool GebruikerExists(string id)
        {
            return (_userManager.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

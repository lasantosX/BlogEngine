using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBlogEngine.Data;
using WebApiBlogEngine.Models;

namespace WebApiBlogEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/<RolesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Roles>>> GetRoles()
        {
            return await _context.Roles.ToListAsync();
        }


        // GET: api/RolesController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Roles>> GetRoles(long id)
        {
            var appointments = await _context.Roles.FindAsync(id);

            if (appointments == null)
            {
                return NotFound();
            }

            return appointments;
        }

        // POST api/<RolesController>
        [HttpPost]
        public async Task<ActionResult<Roles>> PostRoles(Roles Roles)
        {
            _context.Roles.Add(Roles);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRoles", new { id = Roles.idRole }, Roles);
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRoles(long id, Roles Roles)
        {
            if (id != Roles.idRole)
            {
                return BadRequest();
            }

            _context.Entry(Roles).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RolesExists(id))
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

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoles(long id)
        {
            var Roles = await _context.Roles.FindAsync(id);
            if (Roles == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(Roles);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RolesExists(long id)
        {
            return _context.Roles.Any(e => e.idRole == id);
        }

    }
}


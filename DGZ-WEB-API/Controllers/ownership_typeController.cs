using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DGZ_WEB_API;
using DGZ_WEB_API.Models;

namespace DGZ_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ownership_typeController : ControllerBase
    {
        private readonly EFDbContext _context;

        public ownership_typeController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/ownership_type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ownership_type>>> Getownership_types()
        {
            return await _context.ownership_types.ToListAsync();
        }

        // GET: api/ownership_type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ownership_type>> Getownership_type(int id)
        {
            var ownership_type = await _context.ownership_types.FindAsync(id);

            if (ownership_type == null)
            {
                return NotFound();
            }

            return ownership_type;
        }

        // PUT: api/ownership_type/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putownership_type(int id, ownership_type ownership_type)
        {
            if (id != ownership_type.id)
            {
                return BadRequest();
            }

            _context.Entry(ownership_type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ownership_typeExists(id))
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

        // POST: api/ownership_type
        [HttpPost]
        public async Task<ActionResult<ownership_type>> Postownership_type(ownership_type ownership_type)
        {
            _context.ownership_types.Add(ownership_type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getownership_type", new { id = ownership_type.id }, ownership_type);
        }

        // DELETE: api/ownership_type/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ownership_type>> Deleteownership_type(int id)
        {
            var ownership_type = await _context.ownership_types.FindAsync(id);
            if (ownership_type == null)
            {
                return NotFound();
            }

            _context.ownership_types.Remove(ownership_type);
            await _context.SaveChangesAsync();

            return ownership_type;
        }

        private bool ownership_typeExists(int id)
        {
            return _context.ownership_types.Any(e => e.id == id);
        }
    }
}

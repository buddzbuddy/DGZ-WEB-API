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
    [Microsoft.AspNetCore.Cors.EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]")]
    [ApiController]
    public class counterpart_typeController : ControllerBase
    {
        private readonly EFDbContext _context;

        public counterpart_typeController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/counterpart_type
        [HttpGet]
        public async Task<ActionResult<IEnumerable<counterpart_type>>> Getcounterpart_types()
        {
            return await _context.counterpart_types.ToListAsync();
        }

        // GET: api/counterpart_type/5
        [HttpGet("{id}")]
        public async Task<ActionResult<counterpart_type>> Getcounterpart_type(int id)
        {
            var counterpart_type = await _context.counterpart_types.FindAsync(id);

            if (counterpart_type == null)
            {
                return NotFound();
            }

            return counterpart_type;
        }

        // PUT: api/counterpart_type/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putcounterpart_type(int id, counterpart_type counterpart_type)
        {
            if (id != counterpart_type.id)
            {
                return BadRequest();
            }

            _context.Entry(counterpart_type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!counterpart_typeExists(id))
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

        // POST: api/counterpart_type
        [HttpPost]
        public async Task<ActionResult<counterpart_type>> Postcounterpart_type(counterpart_type counterpart_type)
        {
            _context.counterpart_types.Add(counterpart_type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcounterpart_type", new { id = counterpart_type.id }, counterpart_type);
        }

        // DELETE: api/counterpart_type/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<counterpart_type>> Deletecounterpart_type(int id)
        {
            var counterpart_type = await _context.counterpart_types.FindAsync(id);
            if (counterpart_type == null)
            {
                return NotFound();
            }

            _context.counterpart_types.Remove(counterpart_type);
            await _context.SaveChangesAsync();

            return counterpart_type;
        }

        private bool counterpart_typeExists(int id)
        {
            return _context.counterpart_types.Any(e => e.id == id);
        }
    }
}

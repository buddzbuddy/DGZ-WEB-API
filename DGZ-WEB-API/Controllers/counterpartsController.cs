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
    public class counterpartsController : ControllerBase
    {
        private readonly EFDbContext _context;

        public counterpartsController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/counterparts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<counterpart>>> Getcounterparts()
        {
            return await _context.counterparts.ToListAsync();
        }

        // GET: api/counterparts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<counterpart>> Getcounterpart(int id)
        {
            var counterpart = await _context.counterparts.FindAsync(id);

            if (counterpart == null)
            {
                return NotFound();
            }

            return counterpart;
        }

        // PUT: api/counterparts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putcounterpart(int id, counterpart counterpart)
        {
            if (id != counterpart.id)
            {
                return BadRequest();
            }

            _context.Entry(counterpart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!counterpartExists(id))
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

        // POST: api/counterparts
        [HttpPost]
        public async Task<ActionResult<counterpart>> Postcounterpart(counterpart counterpart)
        {
            _context.counterparts.Add(counterpart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcounterpart", new { id = counterpart.id }, counterpart);
        }

        // DELETE: api/counterparts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<counterpart>> Deletecounterpart(int id)
        {
            var counterpart = await _context.counterparts.FindAsync(id);
            if (counterpart == null)
            {
                return NotFound();
            }

            _context.counterparts.Remove(counterpart);
            await _context.SaveChangesAsync();

            return counterpart;
        }

        private bool counterpartExists(int id)
        {
            return _context.counterparts.Any(e => e.id == id);
        }
    }
}

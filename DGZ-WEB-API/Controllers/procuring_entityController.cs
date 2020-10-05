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
    public class procuring_entityController : ControllerBase
    {
        private readonly EFDbContext _context;

        public procuring_entityController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/procuring_entity
        [HttpGet]
        public async Task<ActionResult<IEnumerable<procuring_entity>>> Getprocuring_entities()
        {
            return await _context.procuring_entities.ToListAsync();
        }

        // GET: api/procuring_entity/5
        [HttpGet("{id}")]
        public async Task<ActionResult<procuring_entity>> Getprocuring_entity(int id)
        {
            var procuring_entity = await _context.procuring_entities.FindAsync(id);

            if (procuring_entity == null)
            {
                return NotFound();
            }

            return procuring_entity;
        }

        // PUT: api/procuring_entity/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putprocuring_entity(int id, procuring_entity procuring_entity)
        {
            if (id != procuring_entity.id)
            {
                return BadRequest();
            }

            _context.Entry(procuring_entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!procuring_entityExists(id))
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

        // POST: api/procuring_entity
        [HttpPost]
        public async Task<ActionResult<procuring_entity>> Postprocuring_entity(procuring_entity procuring_entity)
        {
            _context.procuring_entities.Add(procuring_entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getprocuring_entity", new { id = procuring_entity.id }, procuring_entity);
        }

        // DELETE: api/procuring_entity/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<procuring_entity>> Deleteprocuring_entity(int id)
        {
            var procuring_entity = await _context.procuring_entities.FindAsync(id);
            if (procuring_entity == null)
            {
                return NotFound();
            }

            _context.procuring_entities.Remove(procuring_entity);
            await _context.SaveChangesAsync();

            return procuring_entity;
        }

        private bool procuring_entityExists(int id)
        {
            return _context.procuring_entities.Any(e => e.id == id);
        }
    }
}

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
    public class audit_method_typesController : ControllerBase
    {
        private readonly EFDbContext _context;

        public audit_method_typesController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<audit_method_type>>> Getaudit_method_types()
        {
            return await _context.audit_method_types.ToListAsync();
        }

        // GET: api/countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<audit_method_type>> Getaudit_method_type(int id)
        {
            var audit_method_type = await _context.audit_method_types.FindAsync(id);

            if (audit_method_type == null)
            {
                return NotFound();
            }

            return audit_method_type;
        }

        // PUT: api/countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putaudit_method_type(int id, audit_method_type audit_method_type)
        {
            if (id != audit_method_type.id)
            {
                return BadRequest();
            }

            _context.Entry(audit_method_type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!audit_method_typeExists(id))
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

        // POST: api/countries
        [HttpPost]
        public async Task<ActionResult<audit_method_type>> Postaudit_method_type(audit_method_type audit_method_type)
        {
            _context.audit_method_types.Add(audit_method_type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getaudit_method_type", new { id = audit_method_type.id }, audit_method_type);
        }

        // DELETE: api/countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<audit_method_type>> Deleteaudit_method_type(int id)
        {
            var audit_method_type = await _context.audit_method_types.FindAsync(id);
            if (audit_method_type == null)
            {
                return NotFound();
            }

            _context.audit_method_types.Remove(audit_method_type);
            await _context.SaveChangesAsync();

            return audit_method_type;
        }

        private bool audit_method_typeExists(int id)
        {
            return _context.countries.Any(e => e.id == id);
        }
    }
}

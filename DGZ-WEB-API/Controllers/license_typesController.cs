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
    public class license_typesController : ControllerBase
    {
        private readonly EFDbContext _context;

        public license_typesController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<license_type>>> Getlicense_types()
        {
            return await _context.license_types.ToListAsync();
        }

        // GET: api/countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<license_type>> Getlicense_type(int id)
        {
            var license_type = await _context.license_types.FindAsync(id);

            if (license_type == null)
            {
                return NotFound();
            }

            return license_type;
        }

        // PUT: api/countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putlicense_type(int id, license_type license_type)
        {
            if (id != license_type.id)
            {
                return BadRequest();
            }

            _context.Entry(license_type).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!license_typeExists(id))
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
        public async Task<ActionResult<license_type>> Postlicense_type(license_type license_type)
        {
            _context.license_types.Add(license_type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getlicense_type", new { id = license_type.id }, license_type);
        }

        // DELETE: api/countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<license_type>> Deletelicense_type(int id)
        {
            var license_type = await _context.license_types.FindAsync(id);
            if (license_type == null)
            {
                return NotFound();
            }

            _context.license_types.Remove(license_type);
            await _context.SaveChangesAsync();

            return license_type;
        }

        private bool license_typeExists(int id)
        {
            return _context.countries.Any(e => e.id == id);
        }
    }
}

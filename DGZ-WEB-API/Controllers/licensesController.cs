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
    public class licensesController : ControllerBase
    {
        private readonly EFDbContext _context;

        public licensesController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<license>>> Getlicenses()
        {
            return await _context.licenses.ToListAsync();
        }

        // GET: api/countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<license>> Getlicense(int id)
        {
            var license = await _context.licenses.FindAsync(id);

            if (license == null)
            {
                return NotFound();
            }

            return license;
        }

        // PUT: api/countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putlicense(int id, license license)
        {
            if (id != license.id)
            {
                return BadRequest();
            }

            _context.Entry(license).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!licenseExists(id))
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
        public async Task<ActionResult<license>> Postlicense(license license)
        {
            _context.licenses.Add(license);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getlicense", new { id = license.id }, license);
        }

        // DELETE: api/countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<license>> Deletelicense(int id)
        {
            var license = await _context.licenses.FindAsync(id);
            if (license == null)
            {
                return NotFound();
            }

            _context.licenses.Remove(license);
            await _context.SaveChangesAsync();

            return license;
        }

        private bool licenseExists(int id)
        {
            return _context.countries.Any(e => e.id == id);
        }
    }
}

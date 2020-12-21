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
    public class countriesController : ControllerBase
    {
        private readonly EFDbContext _context;

        public countriesController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<country>>> Getcountries()
        {
            return await _context.countries.ToListAsync();
        }

        // GET: api/countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<country>> Getcountry(int id)
        {
            var country = await _context.countries.FindAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            return country;
        }

        // PUT: api/countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putcountry(int id, country country)
        {
            if (id != country.id)
            {
                return BadRequest();
            }

            _context.Entry(country).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!countryExists(id))
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
        public async Task<ActionResult<country>> Postcountry(country country)
        {
            _context.countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcountry", new { id = country.id }, country);
        }

        // DELETE: api/countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<country>> Deletecountry(int id)
        {
            var country = await _context.countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.countries.Remove(country);
            await _context.SaveChangesAsync();

            return country;
        }

        private bool countryExists(int id)
        {
            return _context.countries.Any(e => e.id == id);
        }
    }
}

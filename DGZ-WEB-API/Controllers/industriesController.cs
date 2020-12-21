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
    public class industriesController : ControllerBase
    {
        private readonly EFDbContext _context;

        public industriesController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<industry>>> Getcountries()
        {
            return await _context.industries.ToListAsync();
        }

        // GET: api/countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<industry>> Getmember_type(int id)
        {
            var industry = await _context.industries.FindAsync(id);

            if (industry == null)
            {
                return NotFound();
            }

            return industry;
        }

        // PUT: api/countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putmember_type(int id, industry industry)
        {
            if (id != industry.id)
            {
                return BadRequest();
            }

            _context.Entry(industry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!member_typeExists(id))
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
        public async Task<ActionResult<industry>> Postmember_type(industry industry)
        {
            _context.industries.Add(industry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmember_type", new { id = industry.id }, industry);
        }

        // DELETE: api/countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<industry>> Deletemember_type(int id)
        {
            var industry = await _context.industries.FindAsync(id);
            if (industry == null)
            {
                return NotFound();
            }

            _context.industries.Remove(industry);
            await _context.SaveChangesAsync();

            return industry;
        }

        private bool member_typeExists(int id)
        {
            return _context.countries.Any(e => e.id == id);
        }
    }
}

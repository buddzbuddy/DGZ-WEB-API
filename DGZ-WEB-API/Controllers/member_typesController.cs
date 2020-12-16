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
    public class member_typesController : ControllerBase
    {
        private readonly EFDbContext _context;

        public member_typesController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<member_type>>> Getcountries()
        {
            return await _context.member_types.ToListAsync();
        }

        // GET: api/countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<member_type>> Getmember_type(int id)
        {
            var member_type = await _context.member_types.FindAsync(id);

            if (member_type == null)
            {
                return NotFound();
            }

            return member_type;
        }

        // PUT: api/countries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putmember_type(int id, member_type member_type)
        {
            if (id != member_type.id)
            {
                return BadRequest();
            }

            _context.Entry(member_type).State = EntityState.Modified;

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
        public async Task<ActionResult<member_type>> Postmember_type(member_type member_type)
        {
            _context.member_types.Add(member_type);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmember_type", new { id = member_type.id }, member_type);
        }

        // DELETE: api/countries/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<member_type>> Deletemember_type(int id)
        {
            var member_type = await _context.member_types.FindAsync(id);
            if (member_type == null)
            {
                return NotFound();
            }

            _context.member_types.Remove(member_type);
            await _context.SaveChangesAsync();

            return member_type;
        }

        private bool member_typeExists(int id)
        {
            return _context.countries.Any(e => e.id == id);
        }
    }
}

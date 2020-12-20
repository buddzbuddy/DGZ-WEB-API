using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DGZ_WEB_API;
using DGZ_WEB_API.Models;
using Microsoft.AspNetCore.Cors;

namespace DGZ_WEB_API.Controllers
{
    [EnableCors("_myAllowSpecificOrigins")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class appealsController : ControllerBase
    {
        private readonly EFDbContext _context;

        public appealsController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<appeal>>> GetAll()
        {
            return await _context.appeals.ToListAsync();
        }

        // GET: api/countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<appeal>>> GetBySupplier(int supplier)
        {
            return await _context.appeals.Include(x => x._procuring_entity).Where(x => x.supplier == supplier).ToListAsync();
        }

        // GET: api/countries/5
        [HttpGet]
        public async Task<ActionResult<appeal>> Get(int id)
        {
            var appeal = await _context.appeals.FindAsync(id);

            if (appeal == null)
            {
                return NotFound();
            }

            return appeal;
        }

        // PUT: api/countries/5
        [HttpPut]
        public async Task<IActionResult> Put(int id, [FromBody]appeal appeal)
        {
            if (id != appeal.id)
            {
                return BadRequest();
            }

            _context.Entry(appeal).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!appealExists(id))
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
        public async Task<ActionResult<appeal>> Post([FromBody]appeal appeal)
        {
            _context.appeals.Add(appeal);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Get", new { id = appeal.id }, appeal);
        }

        // DELETE: api/countries/5
        [HttpDelete]
        public async Task<ActionResult<appeal>> Delete(int id)
        {
            var appeal = await _context.appeals.FindAsync(id);
            if (appeal == null)
            {
                return NotFound();
            }

            _context.appeals.Remove(appeal);
            await _context.SaveChangesAsync();

            return appeal;
        }

        private bool appealExists(int id)
        {
            return _context.countries.Any(e => e.id == id);
        }
    }
}

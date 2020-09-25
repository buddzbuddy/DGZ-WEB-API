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
    public class taxe_codeController : ControllerBase
    {
        private readonly EFDbContext _context;

        public taxe_codeController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/taxe_code
        [HttpGet]
        public async Task<ActionResult<IEnumerable<taxe_code>>> Gettaxe_codes()
        {
            return await _context.taxe_codes.ToListAsync();
        }

        // GET: api/taxe_code/5
        [HttpGet("{id}")]
        public async Task<ActionResult<taxe_code>> Gettaxe_code(int id)
        {
            var taxe_code = await _context.taxe_codes.FindAsync(id);

            if (taxe_code == null)
            {
                return NotFound();
            }

            return taxe_code;
        }

        // PUT: api/taxe_code/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Puttaxe_code(int id, taxe_code taxe_code)
        {
            if (id != taxe_code.id)
            {
                return BadRequest();
            }

            _context.Entry(taxe_code).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!taxe_codeExists(id))
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

        // POST: api/taxe_code
        [HttpPost]
        public async Task<ActionResult<taxe_code>> Posttaxe_code(taxe_code taxe_code)
        {
            _context.taxe_codes.Add(taxe_code);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Gettaxe_code", new { id = taxe_code.id }, taxe_code);
        }

        // DELETE: api/taxe_code/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<taxe_code>> Deletetaxe_code(int id)
        {
            var taxe_code = await _context.taxe_codes.FindAsync(id);
            if (taxe_code == null)
            {
                return NotFound();
            }

            _context.taxe_codes.Remove(taxe_code);
            await _context.SaveChangesAsync();

            return taxe_code;
        }

        private bool taxe_codeExists(int id)
        {
            return _context.taxe_codes.Any(e => e.id == id);
        }
    }
}

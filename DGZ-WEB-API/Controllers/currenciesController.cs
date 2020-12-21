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
    public class currenciesController : ControllerBase
    {
        private readonly EFDbContext _context;

        public currenciesController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/currencies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<currency>>> Getcurrencies()
        {
            return await _context.currencies.ToListAsync();
        }

        // GET: api/currencies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<currency>> Getcurrency(int id)
        {
            var currency = await _context.currencies.FindAsync(id);

            if (currency == null)
            {
                return NotFound();
            }

            return currency;
        }

        // PUT: api/currencies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putcurrency(int id, currency currency)
        {
            if (id != currency.id)
            {
                return BadRequest();
            }

            _context.Entry(currency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!currencyExists(id))
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

        // POST: api/currencies
        [HttpPost]
        public async Task<ActionResult<currency>> Postcurrency(currency currency)
        {
            _context.currencies.Add(currency);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcurrency", new { id = currency.id }, currency);
        }

        // DELETE: api/currencies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<currency>> Deletecurrency(int id)
        {
            var currency = await _context.currencies.FindAsync(id);
            if (currency == null)
            {
                return NotFound();
            }

            _context.currencies.Remove(currency);
            await _context.SaveChangesAsync();

            return currency;
        }

        private bool currencyExists(int id)
        {
            return _context.currencies.Any(e => e.id == id);
        }
    }
}

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
    public class suppliersController : ControllerBase
    {
        private readonly EFDbContext _context;

        public suppliersController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<supplier>>> Getsupplier()
        {
            return await _context.suppliers.ToListAsync();
        }

        // GET: api/suppliers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<supplier>> Getsupplier(int id)
        {
            var supplier = await _context.suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            return supplier;
        }

        // PUT: api/suppliers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putsupplier(int id, supplier supplier)
        {
            if (id != supplier.id)
            {
                return BadRequest();
            }

            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!supplierExists(id))
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

        // POST: api/suppliers
        [HttpPost]
        public async Task<ActionResult<supplier>> Postsupplier(supplier supplier)
        {
            _context.suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getsupplier", new { id = supplier.id }, supplier);
        }

        // DELETE: api/suppliers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<supplier>> Deletesupplier(int id)
        {
            var supplier = await _context.suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            _context.suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return supplier;
        }

        private bool supplierExists(int id)
        {
            return _context.suppliers.Any(e => e.id == id);
        }
    }
}

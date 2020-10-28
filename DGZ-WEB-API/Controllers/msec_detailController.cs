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
    public class msec_detailController : ControllerBase
    {
        private readonly EFDbContext _context;

        public msec_detailController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/msec_detail/5
        [HttpGet("msec_detailBySupplierMember/{supplier_member}")]
        public async Task<ActionResult<msec_detail[]>> Getmsec_detailBySupplierMember(int supplier_member)
        {
            return await _context.msec_details.Where(x => x.supplier_member == supplier_member).ToArrayAsync();
        }


        // GET: api/msec_detail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<msec_detail>>> Getmsec_details()
        {
            return await _context.msec_details.ToListAsync();
        }

        // GET: api/msec_detail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<msec_detail>> Getmsec_detail(int id)
        {
            var msec_detail = await _context.msec_details.FindAsync(id);

            if (msec_detail == null)
            {
                return NotFound();
            }

            return msec_detail;
        }


        // PUT: api/msec_detail/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putmsec_detail(int id, msec_detail msec_detail)
        {
            if (id != msec_detail.id)
            {
                return BadRequest();
            }

            _context.Entry(msec_detail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!msec_detailExists(id))
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

        // POST: api/msec_detail
        [HttpPost]
        public async Task<ActionResult<msec_detail>> Postmsec_detail(msec_detail msec_detail)
        {
            _context.msec_details.Add(msec_detail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmsec_detail", new { id = msec_detail.id }, msec_detail);
        }

        // DELETE: api/msec_detail/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<msec_detail>> Deletemsec_detail(int id)
        {
            var msec_detail = await _context.msec_details.FindAsync(id);
            if (msec_detail == null)
            {
                return NotFound();
            }

            _context.msec_details.Remove(msec_detail);
            await _context.SaveChangesAsync();

            return msec_detail;
        }

        private bool msec_detailExists(int id)
        {
            return _context.msec_details.Any(e => e.id == id);
        }
    }
}

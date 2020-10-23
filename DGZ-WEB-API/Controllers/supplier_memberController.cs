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
    public class supplier_memberController : ControllerBase
    {
        private readonly EFDbContext _context;

        public supplier_memberController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/supplier_member
        [HttpGet]
        public async Task<ActionResult<IEnumerable<supplier_member>>> Getsupplier_members()
        {
            return await _context.supplier_members.ToListAsync();
        }

        // GET: api/supplier_member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<supplier_member>> Getsupplier_member(int id)
        {
            var supplier_member = await _context.supplier_members.FindAsync(id);

            if (supplier_member == null)
            {
                return NotFound();
            }

            return supplier_member;
        }

        // PUT: api/supplier_member/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Putsupplier_member(int id, supplier_member supplier_member)
        {
            if (id != supplier_member.id)
            {
                return BadRequest();
            }

            _context.Entry(supplier_member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!supplier_memberExists(id))
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

        // POST: api/supplier_member
        [HttpPost]
        public async Task<ActionResult<supplier_member>> Postsupplier_member(supplier_member supplier_member)
        {
            if(_context.supplier_members.Any(x => x.supplier == supplier_member.supplier))
            {
                return Ok(new { error = "Гражданин с таким ПИН уже существует" });
            }

            _context.supplier_members.Add(supplier_member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getsupplier_member", new { id = supplier_member.id }, supplier_member);
        }

        // DELETE: api/supplier_member/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<supplier_member>> Deletesupplier_member(int id)
        {
            var supplier_member = await _context.supplier_members.FindAsync(id);
            if (supplier_member == null)
            {
                return NotFound();
            }

            _context.supplier_members.Remove(supplier_member);
            await _context.SaveChangesAsync();

            return supplier_member;
        }

        private bool supplier_memberExists(int id)
        {
            return _context.supplier_members.Any(e => e.id == id);
        }
    }
}

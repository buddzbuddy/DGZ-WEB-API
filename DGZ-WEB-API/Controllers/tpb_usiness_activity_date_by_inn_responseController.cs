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
    public class tpb_usiness_activity_date_by_inn_responseController : ControllerBase
    {
        private readonly EFDbContext _context;

        public tpb_usiness_activity_date_by_inn_responseController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/tpb_usiness_activity_date_by_inn_response
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tpb_usiness_activity_date_by_inn_response>>> Gettpb_usiness_activity_date_by_inn_responses()
        {
            return await _context.tpb_usiness_activity_date_by_inn_responses.ToListAsync();
        }

        // GET: api/tpb_usiness_activity_date_by_inn_response/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tpb_usiness_activity_date_by_inn_response>> Gettpb_usiness_activity_date_by_inn_response(int id)
        {
            var tpb_usiness_activity_date_by_inn_response = await _context.tpb_usiness_activity_date_by_inn_responses.FindAsync(id);

            if (tpb_usiness_activity_date_by_inn_response == null)
            {
                return NotFound();
            }

            return tpb_usiness_activity_date_by_inn_response;
        }

        // PUT: api/tpb_usiness_activity_date_by_inn_response/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Puttpb_usiness_activity_date_by_inn_response(int id, tpb_usiness_activity_date_by_inn_response tpb_usiness_activity_date_by_inn_response)
        {
            if (id != tpb_usiness_activity_date_by_inn_response.id)
            {
                return BadRequest();
            }

            _context.Entry(tpb_usiness_activity_date_by_inn_response).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tpb_usiness_activity_date_by_inn_responseExists(id))
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

        // POST: api/tpb_usiness_activity_date_by_inn_response
        [HttpPost]
        public async Task<ActionResult<tpb_usiness_activity_date_by_inn_response>> Posttpb_usiness_activity_date_by_inn_response(tpb_usiness_activity_date_by_inn_response tpb_usiness_activity_date_by_inn_response)
        {
            _context.tpb_usiness_activity_date_by_inn_responses.Add(tpb_usiness_activity_date_by_inn_response);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Gettpb_usiness_activity_date_by_inn_response", new { id = tpb_usiness_activity_date_by_inn_response.id }, tpb_usiness_activity_date_by_inn_response);
        }

        // DELETE: api/tpb_usiness_activity_date_by_inn_response/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<tpb_usiness_activity_date_by_inn_response>> Deletetpb_usiness_activity_date_by_inn_response(int id)
        {
            var tpb_usiness_activity_date_by_inn_response = await _context.tpb_usiness_activity_date_by_inn_responses.FindAsync(id);
            if (tpb_usiness_activity_date_by_inn_response == null)
            {
                return NotFound();
            }

            _context.tpb_usiness_activity_date_by_inn_responses.Remove(tpb_usiness_activity_date_by_inn_response);
            await _context.SaveChangesAsync();

            return tpb_usiness_activity_date_by_inn_response;
        }

        private bool tpb_usiness_activity_date_by_inn_responseExists(int id)
        {
            return _context.tpb_usiness_activity_date_by_inn_responses.Any(e => e.id == id);
        }
    }
}

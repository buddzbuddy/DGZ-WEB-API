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
    public class tp_data_by_inn_for_business_activity_responseController : ControllerBase
    {
        private readonly EFDbContext _context;

        public tp_data_by_inn_for_business_activity_responseController(EFDbContext context)
        {
            _context = context;
        }

        // GET: api/tp_data_by_inn_for_business_activity_response
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tp_data_by_inn_for_business_activity_response>>> Gettp_data_by_inn_for_business_activity_responses()
        {
            return await _context.tp_data_by_inn_for_business_activity_responses.ToListAsync();
        }

        // GET: api/tp_data_by_inn_for_business_activity_response/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tp_data_by_inn_for_business_activity_response>> Gettp_data_by_inn_for_business_activity_response(int id)
        {
            var tp_data_by_inn_for_business_activity_response = await _context.tp_data_by_inn_for_business_activity_responses.FindAsync(id);

            if (tp_data_by_inn_for_business_activity_response == null)
            {
                return NotFound();
            }

            return tp_data_by_inn_for_business_activity_response;
        }

        // PUT: api/tp_data_by_inn_for_business_activity_response/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Puttp_data_by_inn_for_business_activity_response(int id, tp_data_by_inn_for_business_activity_response tp_data_by_inn_for_business_activity_response)
        {
            if (id != tp_data_by_inn_for_business_activity_response.id)
            {
                return BadRequest();
            }

            _context.Entry(tp_data_by_inn_for_business_activity_response).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tp_data_by_inn_for_business_activity_responseExists(id))
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

        // POST: api/tp_data_by_inn_for_business_activity_response
        [HttpPost]
        public async Task<ActionResult<tp_data_by_inn_for_business_activity_response>> Posttp_data_by_inn_for_business_activity_response(tp_data_by_inn_for_business_activity_response tp_data_by_inn_for_business_activity_response)
        {
            _context.tp_data_by_inn_for_business_activity_responses.Add(tp_data_by_inn_for_business_activity_response);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Gettp_data_by_inn_for_business_activity_response", new { id = tp_data_by_inn_for_business_activity_response.id }, tp_data_by_inn_for_business_activity_response);
        }

        // DELETE: api/tp_data_by_inn_for_business_activity_response/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<tp_data_by_inn_for_business_activity_response>> Deletetp_data_by_inn_for_business_activity_response(int id)
        {
            var tp_data_by_inn_for_business_activity_response = await _context.tp_data_by_inn_for_business_activity_responses.FindAsync(id);
            if (tp_data_by_inn_for_business_activity_response == null)
            {
                return NotFound();
            }

            _context.tp_data_by_inn_for_business_activity_responses.Remove(tp_data_by_inn_for_business_activity_response);
            await _context.SaveChangesAsync();

            return tp_data_by_inn_for_business_activity_response;
        }

        private bool tp_data_by_inn_for_business_activity_responseExists(int id)
        {
            return _context.tp_data_by_inn_for_business_activity_responses.Any(e => e.id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hexa_Hub.Interface;
using Hexa_Hub.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReturnRequestsController : ControllerBase
    {
        private readonly IReturnReqRepo _returnRequestRepo;
        private readonly DataContext _context;

        public ReturnRequestsController(DataContext context, IReturnReqRepo returnRequestRepo)
        {
            _context = context;
            _returnRequestRepo = returnRequestRepo;
        }

        // GET: api/ReturnRequests
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ReturnRequest>>> GetReturnRequests()
        {
            //return await _context.ReturnRequests.ToListAsync();
            return await _returnRequestRepo.GetAllReturnRequest();
        }

        // GET: api/ReturnRequests/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ReturnRequest>> GetReturnRequest(int id)
        {
            //var returnRequest = await _context.ReturnRequests.FindAsync(id);
            var returnRequest = await _returnRequestRepo.GetReturnRequestById(id);

            if (returnRequest == null)
            {
                return NotFound();
            }

            return returnRequest;
        }

        // PUT: api/ReturnRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutReturnRequest(int id, ReturnRequest returnRequest)
        {
            if (id != returnRequest.ReturnId)
            {
                return BadRequest();
            }

            //_context.Entry(returnRequest).State = EntityState.Modified;
            _returnRequestRepo.UpdateReturnRequest(returnRequest);

            try
            {
                //await _context.SaveChangesAsync();
                await _returnRequestRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReturnRequestExists(id))
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

        // POST: api/ReturnRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReturnRequest>> PostReturnRequest(ReturnRequest returnRequest)
        {
            //_context.ReturnRequests.Add(returnRequest);
            //await _context.SaveChangesAsync();
            _returnRequestRepo.AddReturnRequest(returnRequest);
            await _returnRequestRepo.Save();

            return CreatedAtAction("GetReturnRequest", new { id = returnRequest.ReturnId }, returnRequest);
        }

        // DELETE: api/ReturnRequests/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReturnRequest(int id)
        {
            //var returnRequest = await _context.ReturnRequests.FindAsync(id);
            //if (returnRequest == null)
            //{
            //    return NotFound();
            //}

            //_context.ReturnRequests.Remove(returnRequest);
            //await _context.SaveChangesAsync();

            //return NoContent();
            try
            {
                await _returnRequestRepo.DeleteReturnRequest(id);
                await _returnRequestRepo.Save();
                return NoContent();
            }
            catch (Exception)
            {
                if (id == null)
                    return NotFound();
                return BadRequest();
            }
        }

        private bool ReturnRequestExists(int id)
        {
            return _context.ReturnRequests.Any(e => e.ReturnId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class AuditsController : ControllerBase
    {
        private readonly IAuditRepo _auditRepo;
        private readonly DataContext _context;

        public AuditsController(IAuditRepo auditRepo,DataContext context)
        {
            _auditRepo = auditRepo;
            _context = context;
        }

        // GET: api/Audits
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Audit>>> GetAudits()
        {
            //return await _context.Audits.ToListAsync();
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "Admin")
            {
                return await _auditRepo.GetAllAudits();
            }
            else
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var audit = await _auditRepo.GetAuditById(userId);
                if (audit == null)
                {
                    return NotFound();
                }
                return Ok(new List<Audit> { audit });
            }
        }

        // GET: api/Audits/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Audit>> GetAudit(int id)
        {
            //var audit = await _context.Audits.FindAsync(id);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "Admin")
            {
                var audit = await _auditRepo.GetAuditById(id);
                if (audit == null)
                {
                    return NotFound();
                }
                return audit;
            }
            else
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var audit = await _auditRepo.GetAuditById(userId);
                if (audit == null)
                {
                    return NotFound();
                }
                return Ok(new List<Audit> { audit });
            }
        }

        // PUT: api/Audits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAudit(int id, Audit audit)
        {
            if (id != audit.AuditId)
            {
                return BadRequest();
            }

            //_context.Entry(audit).State = EntityState.Modified;
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var existingId = await _auditRepo.GetAuditById(id);
            if(existingId == null)
            {
                return NotFound();
            }
            if (existingId.UserId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }
            _auditRepo.UpdateAudit(audit);
            try
            {
                //await _context.SaveChangesAsync();
                await _auditRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditExists(id))
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

        // POST: api/Audits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Audit>> PostAudit(Audit audit)
        {
            //_context.Audits.Add(audit);
            //await _context.SaveChangesAsync();
            _auditRepo.AddAuditReq(audit);
            await _auditRepo.Save();

            return CreatedAtAction("GetAudit", new { id = audit.AuditId }, audit);
        }

        // DELETE: api/Audits/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAudit(int id)
        {
            //var audit = await _context.Audits.FindAsync(id);
            //if (audit == null)
            //{
            //    return NotFound();
            //}

            //_context.Audits.Remove(audit);
            //await _context.SaveChangesAsync();

            //return NoContent();
            try
            {
                await _auditRepo.DeleteAuditReq(id);
                await _auditRepo.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        private bool AuditExists(int id)
        {
            return _context.Audits.Any(e => e.AuditId == id);
        }
    }
}

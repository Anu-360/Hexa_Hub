﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hexa_Hub.DTO;
using Hexa_Hub.Exceptions;
using Hexa_Hub.Interface;
using Hexa_Hub.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Hexa_Hub.Models.MultiValues;

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
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "Admin")
            {
                return await _auditRepo.GetAllAudits();
            }
            else
            {
                var req = await _auditRepo.GetAuditsByUserId(userId);
                if (req == null)
                {
                    return NotFound("id not Found");
                }
                return Ok(req);
            }
        }

        // GET: api/Audits/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Audit>> GetAudit(int id)
        {
            //User can see his own details whereas Admin can see all users details
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "Admin")
            {
                var audit = await _auditRepo.GetAuditById(id);
                if (audit == null)
                {
                    return NotFound("id not Found");
                }
                return audit;
            }
            else
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var audit = await _auditRepo.GetAuditById(userId);
                if (audit == null)
                {
                    return NotFound("id not Found");
                }
                return Ok(new List<Audit> { audit });
            }
        }
        // PUT: api/Audits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> PutAudit(int id, [FromBody] AuditsDto auditDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != auditDto.AuditId)
            {
                return BadRequest("Audit ID mismatch.");
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var existingAudit = await _auditRepo.GetAuditById(id);
            if (existingAudit == null)
            {
                return NotFound("id not Found");
            }

            if (existingAudit.UserId != userId)
            {
                return Forbid($"Sorry you are not User {userId}");
            }
            existingAudit.AuditDate = auditDto.AuditDate;
            existingAudit.AuditMessage = auditDto.AuditMessage;
            if (Enum.TryParse<AuditStatus>(auditDto.Audit_Status, out var status))
            {
                existingAudit.Audit_Status = status;
            }
            else
            {
                return BadRequest($"Invalid Audit Status: {auditDto.Audit_Status}");
            }


            try
            {
                await _auditRepo.UpdateAudit(existingAudit);
                await _auditRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuditExists(id))
                {
                    return NotFound("No Audit Exists");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Audit Sent Successfully");
        }


        // POST: api/Audits
        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<Audit>> PostAudit(AuditsDto auditDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var audit = await _auditRepo.AddAduit(auditDto);
            await _auditRepo.Save();

            return CreatedAtAction("GetAudit", new { id = audit.AuditId }, audit);
        }


        // DELETE: api/Audits/5
        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAudit(int id)
        {
            try
            {
                
                await _auditRepo.DeleteAuditReq(id);
                await _auditRepo.Save();
                return Ok("Audit Deleted Successfully");
            }
            catch (AuditNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        private bool AuditExists(int id)
        {
            return _context.Audits.Any(e => e.AuditId == id);
        }
    }
}

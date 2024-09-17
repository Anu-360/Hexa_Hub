using System;
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

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceLogsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMaintenanceLogRepo _maintenanceLogRepo;
        public MaintenanceLogsController(DataContext context, IMaintenanceLogRepo maintenanceLogRepo)
        {
            _context = context;
            _maintenanceLogRepo = maintenanceLogRepo;
        }

        // GET: api/MaintenanceLogs
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MaintenanceLog>>> GetMaintenanceLogs()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "Admin")
            {
                return await _maintenanceLogRepo.GetAllMaintenanceLog();
            }
            else
            {
                var req = await _maintenanceLogRepo.GetMaintenanceLogByUserId(userId);
                if (req == null)
                {
                    return NotFound($"No maintenance has been for for user {userId}");
                }
                return Ok(req);
            }
        }

        [HttpGet("invoice/{maintenanceId}")]
        public async Task<IActionResult> DownloadInvoice(int maintenanceId)
        {
            try
            {
                var pdfBytes = await _maintenanceLogRepo.GenerateMaintenanceInvoicePdfAsync(maintenanceId);

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return NotFound("Invoice could not be generated.");
                }

                return File(pdfBytes, "application/pdf", $"MaintenanceInvoice_{maintenanceId}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/MaintenanceLogs/5
        [HttpGet("{userId}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<MaintenanceLog>> GetMaintenanceLog(int userId)
        {

            var maintenanceLogs = await _maintenanceLogRepo.GetMaintenanceLogById(userId);


            if (maintenanceLogs == null)
            {
                return NotFound($"No maintenance has been for for user {userId}");
            }

            return Ok(maintenanceLogs);
        }

        // PUT: api/MaintenanceLogs/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMaintenanceLog(int id, [FromBody] MaintenanceDto maintenanceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _maintenanceLogRepo.UpdateMaintenanceLog(id, maintenanceDto);
                if (!result)
                {
                    return NotFound($"No maintenance has been for for user {id}");
                }
                await _maintenanceLogRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceLogExists(id))
                {
                    return NotFound($"No maintenance has been for for user {id}");
                }
                else
                {
                    throw;
                }
            }

            return Ok("Updation Success");
        }

        // DELETE: api/MaintenanceLogs/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMaintenanceLog(int id)
        {
            try
            {
                await _maintenanceLogRepo.DeleteMaintenanceLog(id);
                await _maintenanceLogRepo.Save();
                return Ok($"Deletion Occured for id {id}");
            }
            catch (MaintenanceLogNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool MaintenanceLogExists(int id)
        {
            return _context.MaintenanceLogs.Any(e => e.MaintenanceId == id);
        }
    }
}

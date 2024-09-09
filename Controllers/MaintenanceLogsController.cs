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
            //return await _context.MaintenanceLogs.ToListAsync();
            return await _maintenanceLogRepo.GetAllMaintenanceLog();
        }

        // GET: api/MaintenanceLogs/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<MaintenanceLog>> GetMaintenanceLog(int id)
        {
            //var maintenanceLog = await _context.MaintenanceLogs.FindAsync(id);
            var maintenanceLog = await _maintenanceLogRepo.GetMaintenanceLogById(id);

            if (maintenanceLog == null)
            {
                return NotFound();
            }

            return maintenanceLog;
        }

        // PUT: api/MaintenanceLogs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMaintenanceLog(int id, MaintenanceLog maintenanceLog)
        {
            if (id != maintenanceLog.MaintenanceId)
            {
                return BadRequest();
            }

            //_context.Entry(maintenanceLog).State = EntityState.Modified;
            _maintenanceLogRepo.UpdateMaintenanceLog(maintenanceLog);

            try
            {
                //await _context.SaveChangesAsync();
                await _maintenanceLogRepo.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MaintenanceLogExists(id))
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

        // POST: api/MaintenanceLogs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<MaintenanceLog>> PostMaintenanceLog(MaintenanceLog maintenanceLog)
        {
            //_context.MaintenanceLogs.Add(maintenanceLog);
            //await _context.SaveChangesAsync();
            _maintenanceLogRepo.AddMaintenanceLog(maintenanceLog);
            await _maintenanceLogRepo.Save();

            return CreatedAtAction("GetMaintenanceLog", new { id = maintenanceLog.MaintenanceId }, maintenanceLog);
        }

        // DELETE: api/MaintenanceLogs/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMaintenanceLog(int id)
        {
            //var maintenanceLog = await _context.MaintenanceLogs.FindAsync(id);
            //if (maintenanceLog == null)
            //{
            //    return NotFound();
            //}

            //_context.MaintenanceLogs.Remove(maintenanceLog);
            //await _context.SaveChangesAsync();

            //return NoContent();
            try
            {
                await _maintenanceLogRepo.DeleteMaintenanceLog(id);
                await _maintenanceLogRepo.Save();
                return NoContent();
            }
            catch (Exception)
            {
                if (id == null)
                    return NotFound();
                return BadRequest();
            }
        }

        private bool MaintenanceLogExists(int id)
        {
            return _context.MaintenanceLogs.Any(e => e.MaintenanceId == id);
        }
    }
}

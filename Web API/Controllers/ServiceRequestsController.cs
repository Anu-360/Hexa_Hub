using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Hexa_Hub.DTO;
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
    public class ServiceRequestsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IServiceRequest _serviceRequest;
        private readonly IMaintenanceLogRepo _maintenanceLog;

        public ServiceRequestsController(DataContext context, IServiceRequest serviceRequest, IMaintenanceLogRepo maintenanceLog)
        {
            _context = context;
            _serviceRequest = serviceRequest;
            _maintenanceLog = maintenanceLog;
        }

        //// GET: api/ServiceRequests
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole == "Admin")
            {
                return Ok(await _serviceRequest.GetAllServiceRequests());
            }
            else
            {
                var userRequests = await _serviceRequest.GetServiceRequestsByUserId(userId);
                if (userRequests == null || !userRequests.Any())
                {
                    return NotFound("No service requests found for the logged-in user.");
                }
                return Ok(userRequests);
            }
        }

        // PUT: api/ServiceRequests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutServiceRequest(int id, ServiceRequestDto serviceRequestDto)
        {
            if (id != serviceRequestDto.ServiceId)
            {
                return BadRequest();
            }
            var existingRequest = await _serviceRequest.GetServiceRequestById(id);
            existingRequest.AssetId = serviceRequestDto.AssetId;
            existingRequest.UserId = serviceRequestDto.UserId;
            existingRequest.ServiceRequestDate = serviceRequestDto.ServiceRequestDate;
            existingRequest.Issue_Type = serviceRequestDto.Issue_Type;
            existingRequest.ServiceDescription = serviceRequestDto.ServiceDescription;
            existingRequest.ServiceReqStatus = serviceRequestDto.ServiceReqStatus;
            if (serviceRequestDto.ServiceReqStatus == ServiceReqStatus.Approved )
            {
                var asset = await _context.Assets.FindAsync(serviceRequestDto.AssetId);
                if (asset != null)
                {
                    asset.Asset_Status = AssetStatus.UnderMaintenance;
                    _context.Entry(asset).State = EntityState.Modified;
                }
            }
            else if (serviceRequestDto.ServiceReqStatus == ServiceReqStatus.Completed)
            {

                var asset = await _context.Assets.FindAsync(serviceRequestDto.AssetId);
                if (asset != null)
                {
                    asset.Asset_Status = AssetStatus.Allocated;
                    _context.Entry(asset).State = EntityState.Modified;
                }
            }

            try
            {
                _serviceRequest.UpdateServiceRequest(existingRequest);
                await _serviceRequest.Save();
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceRequestExists(id))
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

        // POST: api/ServiceRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<ServiceRequest>> PostServiceRequest(ServiceRequestDto serviceRequestDto)
        {
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            serviceRequestDto.UserId = loggedInUserId;
            var serviceRequest = new ServiceRequest
            {
                AssetId = serviceRequestDto.AssetId,
                UserId = loggedInUserId,
                ServiceRequestDate = serviceRequestDto.ServiceRequestDate,
                Issue_Type = serviceRequestDto.Issue_Type,
                ServiceDescription = serviceRequestDto.ServiceDescription
            };
            await _serviceRequest.AddServiceRequest(serviceRequest);
            await _serviceRequest.Save();

            var maintenanceLog = new MaintenanceLog
            {
                AssetId = serviceRequest.AssetId,
                UserId = loggedInUserId,
                Maintenance_date = DateTime.Now,
                Maintenance_Description = serviceRequest.ServiceDescription
            };

            _maintenanceLog.AddMaintenanceLog(maintenanceLog);
            await _maintenanceLog.Save();

            return CreatedAtAction("GetServiceRequests", new { id = serviceRequest.ServiceId }, serviceRequest);
        }


        // DELETE: api/ServiceRequests/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            try
            {
                var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var serviceRequest = await _serviceRequest.GetServiceRequestById(id);
                if (serviceRequest == null)
                {
                    return NotFound(); 
                }
                if (serviceRequest.UserId != loggedInUserId)
                {
                    return Forbid(); 
                }

                await _serviceRequest.DeleteServiceRequest(id);
                await _serviceRequest.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.ServiceId == id);
        }
    }
}

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
using static Hexa_Hub.Models.MultiValues;

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetRequestsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAssetRequest _assetRequest;
        private readonly IAssetAllocation _assetAlloc;
        private readonly IAsset _asset;
        public AssetRequestsController(DataContext context, IAssetRequest assetRequest, IAssetAllocation assetAlloc, IAsset asset)
        {
            _context = context;
            _assetRequest = assetRequest;
            _assetAlloc = assetAlloc;
            _asset = asset;
        }

        // GET: api/AssetRequests
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AssetRequest>>> GetAssetRequests()
        {
            //User can see his own details whereas Admin can see all users details

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "Admin")
            {
                return await _assetRequest.GetAllAssetRequests();
            }
            else
            {
                var req = await _assetRequest.GetAssetRequestsByUserId(userId);
                if (req == null)
                {
                    return NotFound();
                }
                return Ok(req);
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAssetRequest(int id, [FromBody] AssetRequestDto assetRequestDto)
        {
            
            if (id != assetRequestDto.AssetReqId)
            {
                return BadRequest("Id doesn't Match");
            }

            if (assetRequestDto.Request_Status == RequestStatus.Allocated || assetRequestDto.Request_Status == RequestStatus.Rejected)
            {
                return BadRequest($"The Request ID {id} has been {assetRequestDto.Request_Status}");
            }

            var existingRequest = await _assetRequest.GetAssetRequestById(id);
            if (existingRequest == null)
            {
                return NotFound("Request Not Found");
            }

            try
            {
                await _assetRequest.UpdateAssetRequest(id, assetRequestDto);
                return Ok($"{assetRequestDto.Request_Status} has been Updated");
            }
            catch (AssetRequestNotFoundException)
            {
                return NotFound($"AssetRequest for user {id} Not Found.");
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // POST: api/AssetRequests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<ActionResult<AssetRequest>> PostAssetRequest(AssetRequestDto assetRequestDto)
        {
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            assetRequestDto.UserId = loggedInUserId;
            if (assetRequestDto.UserId != loggedInUserId)
            {
                return Forbid("You can only create a request for yourself.");
            }
            var asset = await _asset.GetAssetById(assetRequestDto.AssetId);
            
            if (asset.Asset_Status == Models.MultiValues.AssetStatus.Allocated || asset.Asset_Status == Models.MultiValues.AssetStatus.UnderMaintenance)
            {
                return StatusCode(403, "The Requested Asset is currently locked (Allocated to another user)");
            }

            await _assetRequest.AddAssetRequest(assetRequestDto);
            await _assetRequest.Save();

            return CreatedAtAction("GetAssetRequests", new { id = assetRequestDto.AssetReqId }, assetRequestDto);
        }

        // DELETE: api/AssetRequests/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> DeleteAssetRequest(int id)
        {
            try
            {
                var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var assetRequest = await _assetRequest.GetAssetRequestById(id);
                if (assetRequest == null)
                {
                    throw new AssetRequestNotFoundException($"AssetRequest for id {assetRequest} not found");
                }
                if (assetRequest.UserId != loggedInUserId)
                {
                    return Forbid("You are not allowed to Delete Request");
                }
                if(assetRequest.Request_Status == RequestStatus.Allocated)
                {
                    return Forbid("The Request has already been Allocated. Please raise an ticket in Return Section if the asset is not needed.");
                }
                await _assetRequest.DeleteAssetRequest(id);
                await _assetRequest.Save();
                return Ok("The Request Has Been Successfully Deleted");
            }
            catch (AssetRequestNotFoundException ex)
            {
                throw new AssetRequestNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        private bool AssetRequestExists(int id)
        {
            return _context.AssetRequests.Any(e => e.AssetReqId == id);
        }
    }
}

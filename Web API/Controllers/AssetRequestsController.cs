﻿using System;
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



        //// PUT: api/AssetRequests/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> PutAssetRequest(int id, AssetRequest assetRequest)
        //{
        //    //when an asset is being set to allocated by admin it automaticaly sets data to AssetAllocation Table
        //    if (id != assetRequest.AssetReqId)
        //    {
        //        return BadRequest();
        //    }
        //    _assetRequest.UpdateAssetRequest(assetRequest);
        //    if (assetRequest.Request_Status == RequestStatus.Allocated)
        //    {
        //        var exisitingAllocId = await _context.AssetAllocations
        //            .FirstOrDefaultAsync(aa => aa.AssetReqId == assetRequest.AssetReqId);
        //        if (exisitingAllocId == null)
        //        {
        //            var assetAllocation = new AssetAllocation
        //            {
        //                AssetId = assetRequest.AssetId,
        //                UserId = assetRequest.UserId,
        //                AssetReqId = assetRequest.AssetReqId,
        //                AllocatedDate = DateTime.Now
        //            };
        //            await _assetAlloc.AddAllocation(assetAllocation);

        //            var asset = await _context.Assets.FindAsync(assetRequest.AssetId);
        //            if (asset != null)
        //            {
        //                asset.Asset_Status = AssetStatus.Allocated;
        //                _asset.UpdateAsset(asset);
        //            }
        //        }
        //    }
        //    try
        //    {
        //        _assetAlloc.Save();
        //        _asset.Save();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AssetRequestExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAssetRequest(int id, [FromBody] AssetRequestDto assetRequestDto)
        {
            if (id != assetRequestDto.AssetReqId)
            {
                return BadRequest();
            }

            var existingRequest = await _assetRequest.GetAssetRequestById(id);
            if (existingRequest == null)
            {
                return NotFound();
            }
            if (assetRequestDto.Request_Status != existingRequest.Request_Status)
            {
                existingRequest.Request_Status = assetRequestDto.Request_Status;

                if (assetRequestDto.Request_Status == RequestStatus.Allocated)
                {
                    var existingAllocId = await _context.AssetAllocations
                        .FirstOrDefaultAsync(aa => aa.AssetReqId == assetRequestDto.AssetReqId);

                    if (existingAllocId == null)
                    {
                        var assetAllocation = new AssetAllocation
                        {
                            AssetId = assetRequestDto.AssetId,
                            UserId = assetRequestDto.UserId,
                            AssetReqId = assetRequestDto.AssetReqId,
                            AllocatedDate = DateTime.Now
                        };
                        await _assetAlloc.AddAllocation(assetAllocation);

                        var asset = await _context.Assets.FindAsync(assetRequestDto.AssetId);
                        if (asset != null)
                        {
                            asset.Asset_Status = AssetStatus.Allocated;
                            _asset.UpdateAsset(asset);
                        }
                    }
                }
            }

            try
            {
                await _assetRequest.Save();
                await _assetAlloc.Save();
                await _asset.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetRequestExists(id))
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
                    return NotFound();
                }
                if (assetRequest.UserId != loggedInUserId)
                {
                    return Forbid();
                }
                if(assetRequest.Request_Status == RequestStatus.Allocated)
                {
                    return Forbid();
                }
                await _assetRequest.DeleteAssetRequest(id);
                await _assetRequest.Save();
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private bool AssetRequestExists(int id)
        {
            return _context.AssetRequests.Any(e => e.AssetReqId == id);
        }
    }
}

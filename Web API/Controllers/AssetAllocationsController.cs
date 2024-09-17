﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Hexa_Hub.Exceptions;
using Hexa_Hub.Interface;
using Hexa_Hub.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hexa_Hub.DTO;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetAllocationsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAssetAllocation _assetallocation;
        //private readonly IUserRepo _userRepo;
        //private readonly IEmail _email;

        public AssetAllocationsController(DataContext context, IAssetAllocation assetAllocation)
     
        {
            _context = context;
            _assetallocation = assetAllocation;
            //_userRepo = userService;
            //_email = emailService;
        }

        // GET: api/AssetAllocations
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AssetAllocation>>> GetAssetAllocations()
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                if (userRole == "Admin")
                {
                    return await _assetallocation.GetAllAllocations();
                }
                else
                {
                    var userRequests = await _assetallocation.GetAllocationListById(userId);
                    if (userRequests == null || !userRequests.Any())
                    {
                        throw new AllocationNotFoundException($"No Allocation can be found for the User {userId}");
                    }
                    return Ok(userRequests);
                }
            }
            catch (AllocationNotFoundException ex)
            {
                throw new AllocationNotFoundException(ex.Message);
            }
        }

        // GET: api/AssetAllocation/filter-by-month?monthName=January
        [HttpGet("filter-by-month")]
        public async Task<IActionResult> FilterAllocationsByMonth(string month)
        {
            // Validate that the month name is not null or empty
            if (string.IsNullOrEmpty(month))
            {
                return BadRequest("Month name is required.");
            }

            try
            {
                var allocations = await _assetallocation.GetAllocationsByMonthAsync(month);

                if (allocations == null || !allocations.Any())
                {
                    throw new AllocationNotFoundException($"No allocations found for the month of {month}.");
                }

                return Ok(allocations);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid month name. Please provide a valid month name (e.g., January, February).");
            }
        }

        // GET: api/AssetAllocation/filter-by-year?year=2024
        [HttpGet("filter-by-year")]
        public async Task<IActionResult> FilterAllocationsByYear(int year)
        {
            if (year < 1900 || year > DateTime.Now.Year)
            {
                return BadRequest("Invalid year. Please provide a valid year.");
            }

            var allocations = await _assetallocation.GetAllocationsByYearAsync(year);

            if (allocations == null || !allocations.Any())
            {
                return NotFound($"No allocations found for the year {year}.");
            }

            return Ok(allocations);
        }

        // GET: api/AssetAllocation/filter-by-month-and-year?month=January&year=2024
        [HttpGet("filter-by-month-and-year")]
        public async Task<IActionResult> FilterAllocationsByMonthAndYear(string month, int year)
        {
            if (string.IsNullOrEmpty(month))
            {
                return BadRequest("Month name is required.");
            }

            if (year < 1900 || year > DateTime.Now.Year)
            {
                return BadRequest("Invalid year. Please provide a valid year.");
            }

            try
            {
                var allocations = await _assetallocation.GetAllocationsByMonthAndYearAsync(month, year);

                if (allocations == null || !allocations.Any())
                {
                   return NotFound($"No allocations found for the month of {month} in the year {year}.");
                }

                return Ok(allocations);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid month name. Please provide a valid month name (e.g., January, February).");
            }
        }

        // GET: api/AssetAllocation/filter-by-date-range?startDate=2024-01-01&endDate=2024-12-31
        [HttpGet("filter-by-date-range")]
        public async Task<IActionResult> FilterAllocationsByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be greater than end date.");
            }

            var allocations = await _assetallocation.GetAllocationsByDateRangeAsync(startDate, endDate);

            if (allocations == null || !allocations.Any())
            {
                throw new AllocationNotFoundException($"No allocations found between {startDate.ToString("yyyy-MM-dd")} and {endDate.ToString("yyyy-MM-dd")}.");
            }

            return Ok(allocations);
        }

        //[HttpPost("allocate")]
        //public async Task<IActionResult> AllocateAsset(AssetAllocationDto allocationDto)
        //{
        //    // Allocate the asset (existing logic)
        //    var assetAllocation = await _assetallocation.AllocateAssetAsync(allocationDto);

        //    if (assetAllocation == null)
        //    {
        //        return BadRequest("Asset allocation failed.");
        //    }

        //    // Fetch employee details
        //    var user = await _userRepo.GetUserById(allocationDto.UserId);

        //    if (user != null)
        //    {
        //        // Prepare email content
        //        string subject = "Asset Allocation Confirmation";
        //        string message = $"Dear {user.UserName},<br/><br/>" +
        //                         $"The following asset has been allocated to you:<br/>" +
        //                         $"<strong>Asset Name:</strong> {assetAllocation.Asset?.AssetName}<br/>" +
        //                         $"<strong>Allocation Date:</strong> {assetAllocation.AllocatedDate:yyyy-MM-dd}<br/><br/>" +
        //                         "Thank you!<br/>Asset Management Team";

        //        // Send the email
        //        await _email.SendEmailAsync(user.UserMail, subject, message);
        //    }

        //    return Ok(assetAllocation);
        //}
    }
}


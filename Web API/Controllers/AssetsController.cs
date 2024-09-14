using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hexa_Hub.Interface;
using Hexa_Hub.Repository;
using System.Security.Claims;
using Hexa_Hub.DTO;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAsset _asset;

        public AssetsController(DataContext context, IAsset asset)

        {
            _context = context;
            _asset = asset;
        }

        // GET: api/Assets
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Asset>>> GetAssets()
        {
            return await _asset.GetAllAssets();
        }
        [HttpGet("Details")]
        [Authorize]
        public async Task<List<Asset>> GetAllDetailsOfAssets()
        {
            return await _asset.GetAllDetailsOfAssets();
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutAsset(int id, [FromBody] AssetDto assetDto)
        {
            if (id != assetDto.AssetId)
            {
                return BadRequest();
            }

            try
            {
                var existingAsset = await _asset.UpdateAssetDto(id, assetDto);
                await _asset.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetExists(id))
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


      
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Asset>> PostAsset([FromBody] AssetDto assetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var asset = await _asset.AddAsset(assetDto);
            return CreatedAtAction("GetAssets", new { id = asset.AssetId }, asset);
        }


        private bool AssetExists(int id)
        {
            return _context.Assets.Any(e => e.AssetId == id);
        }

        [HttpPut("{assetId}/upload")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UploadAssetImage(int assetId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var supportedFiles = new[] { "image/jpeg", "image/png" };
            if (!supportedFiles.Contains(file.ContentType))
            {
                return BadRequest("Only JPEG or PNG format are allowed");
            }
            var fileName = await _asset.UploadAssetImageAsync(assetId, file);
            if (fileName == null)
            {
                return NotFound();
            }

            return Ok(new { FileName = fileName });
        }

        //image
        [HttpGet("{assetId}/assetImage")]
        [Authorize]
        public async Task<IActionResult> GetAssetImage(int assetId)
        {
            var asset = await _asset.GetAssetById(assetId);
            if (asset == null || asset.AssetImage == null)
            {
                var defualtImagePath = _asset.GetImagePath("AssetDefault.jpg");
                return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), defualtImagePath), "image/jpeg");
            }
            string fileName = Encoding.UTF8.GetString(asset.AssetImage);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), _asset.GetImagePath(fileName));

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image file not found.");
            }

            string contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            return PhysicalFile(imagePath, contentType);
        }
    }
}

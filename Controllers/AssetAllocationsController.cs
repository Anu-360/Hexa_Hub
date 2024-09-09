using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetAllocationsController : ControllerBase
    {
        private readonly DataContext _context;

        public AssetAllocationsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/AssetAllocations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AssetAllocation>>> GetAssetAllocations()
        {
            return await _context.AssetAllocations.ToListAsync();
        }

        // GET: api/AssetAllocations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssetAllocation>> GetAssetAllocation(int id)
        {
            var assetAllocation = await _context.AssetAllocations.FindAsync(id);

            if (assetAllocation == null)
            {
                return NotFound();
            }

            return assetAllocation;
        }

        // PUT: api/AssetAllocations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssetAllocation(int id, AssetAllocation assetAllocation)
        {
            if (id != assetAllocation.AllocationId)
            {
                return BadRequest();
            }

            _context.Entry(assetAllocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssetAllocationExists(id))
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

        // POST: api/AssetAllocations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AssetAllocation>> PostAssetAllocation(AssetAllocation assetAllocation)
        {
            _context.AssetAllocations.Add(assetAllocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAssetAllocation", new { id = assetAllocation.AllocationId }, assetAllocation);
        }

        // DELETE: api/AssetAllocations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssetAllocation(int id)
        {
            var assetAllocation = await _context.AssetAllocations.FindAsync(id);
            if (assetAllocation == null)
            {
                return NotFound();
            }

            _context.AssetAllocations.Remove(assetAllocation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssetAllocationExists(int id)
        {
            return _context.AssetAllocations.Any(e => e.AllocationId == id);
        }
    }
}

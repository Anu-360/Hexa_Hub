using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Repository
{
    public class AssetAllocationService : IAssetAllocation
    {
        private readonly DataContext _context;

        public AssetAllocationService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<AssetAllocation>> GetAllAllocations()
        {
            return await _context.AssetAllocations
                                 .Include(a => a.User)
                                 .Include(a => a.Asset)
                                 .Include(a => a.AssetRequests)
                                 .ToListAsync();
        }

        public async Task<AssetAllocation?> GetAllocationById(int id)
        {
            return await _context.AssetAllocations
                                 .Include(a => a.User)
                                 .Include(a => a.Asset)
                                 .Include(a => a.AssetRequests)
                                 .FirstOrDefaultAsync(a => a.AllocationId == id);
        }

        public async Task<AssetAllocation> AddAllocation(AssetAllocation allocation)
        {
            _context.AssetAllocations.Add(allocation);
            await _context.SaveChangesAsync();
            return allocation;
        }

        public async Task<AssetAllocation> UpdateAllocation(AssetAllocation allocation)
        {
            _context.AssetAllocations.Update(allocation);
            await _context.SaveChangesAsync();
            return allocation;
        }

        public async Task<bool> DeleteAllocation(int id)
        {
            var allocation = await _context.AssetAllocations.FindAsync(id);
            if (allocation == null)
            {
                return false;
            }

            _context.AssetAllocations.Remove(allocation);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

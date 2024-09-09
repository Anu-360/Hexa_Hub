using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Repository
{
    public class AssetService : IAsset
    {
        private readonly DataContext _context;

        public AssetService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Asset>> GetAllAssets()
        {
            return await _context.Assets
                                 .Include(a => a.Category)
                                 .Include(a => a.SubCategories)
                                 .Include(a => a.AssetRequests)
                                 .Include(a => a.ServiceRequests)
                                 .Include(a => a.MaintenanceLogs)
                                 .Include(a => a.Audits)
                                 .Include(a => a.ReturnRequests)
                                 .Include(a => a.AssetAllocations)
                                 .ToListAsync();
        }

        public async Task<Asset?> GetAssetById(int id)
        {
            return await _context.Assets
                                 .Include(a => a.Category)
                                 .Include(a => a.SubCategories)
                                 .Include(a => a.AssetRequests)
                                 .Include(a => a.ServiceRequests)
                                 .Include(a => a.MaintenanceLogs)
                                 .Include(a => a.Audits)
                                 .Include(a => a.ReturnRequests)
                                 .Include(a => a.AssetAllocations)
                                 .FirstOrDefaultAsync(a => a.AssetId == id);
        }

        public async Task<Asset> AddAsset(Asset asset)
        {
            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<Asset> UpdateAsset(Asset asset)
        {
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();
            return asset;
        }

        public async Task<bool> DeleteAsset(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return false;
            }

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

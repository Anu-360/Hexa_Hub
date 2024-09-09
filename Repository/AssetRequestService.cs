using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Repository
{
    public class AssetRequestService : IAssetRequest
    {
        private readonly DataContext _context;

        public AssetRequestService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<AssetRequest>> GetAllAssetRequests()
        {
            return await _context.AssetRequests
                                 .Include(ar => ar.Asset)
                                 .Include(ar => ar.User)
                                 .Include(ar => ar.AssetAlocation)
                                 .ToListAsync();
        }

        public async Task<AssetRequest?> GetAssetRequestById(int id)
        {
            return await _context.AssetRequests
                                 .Include(ar => ar.Asset)
                                 .Include(ar => ar.User)
                                 .Include(ar => ar.AssetAlocation)
                                 .FirstOrDefaultAsync(ar => ar.AssetReqId == id);
        }

        public async Task<AssetRequest> AddAssetRequest(AssetRequest assetRequest)
        {
            _context.AssetRequests.Add(assetRequest);
            await _context.SaveChangesAsync();
            return assetRequest;
        }

        public async Task<AssetRequest> UpdateAssetRequest(AssetRequest assetRequest)
        {
            _context.AssetRequests.Update(assetRequest);
            await _context.SaveChangesAsync();
            return assetRequest;
        }

        public async Task<bool> DeleteAssetRequest(int id)
        {
            var assetRequest = await _context.AssetRequests.FindAsync(id);
            if (assetRequest == null)
            {
                return false;
            }

            _context.AssetRequests.Remove(assetRequest);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

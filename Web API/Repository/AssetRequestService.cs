using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;
using Hexa_Hub.Exceptions;
using Hexa_Hub.DTO;

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
                .Include(ar=>ar.Asset)
                .Include(ar=>ar.User)
                .ToListAsync();
        }

        public async Task<List<AssetRequest>> GetAssetRequestsByUserId(int userId)
        {
            return await _context.AssetRequests
                .Where(sr => sr.UserId == userId)
                .Include(sr => sr.Asset)
                .Include(sr => sr.User)
                .ToListAsync();
        }

        public async Task<AssetRequest?> GetAssetRequestById(int id)
        {
            return await _context.AssetRequests
                .Include(ar => ar.Asset)
                .Include(ar => ar.User)
                .FirstOrDefaultAsync(u => u.AssetReqId == id);
        }

        public async Task AddAssetRequest(AssetRequestDto dto)
        {
            var req = new AssetRequest
            {
                AssetReqId = dto.AssetReqId,
                UserId = dto.UserId,
                AssetId = dto.AssetId,
                CategoryId = dto.CategoryId,
                AssetReqDate = dto.AssetReqDate,
                AssetReqReason = dto.AssetReqReason
            };
            _context.AssetRequests.Add(req);
        }

        public Task<AssetRequest> UpdateAssetRequest(AssetRequest assetRequest)
        {
            _context.AssetRequests.Update(assetRequest);
            return Task.FromResult(assetRequest);

        }

        public async Task DeleteAssetRequest(int id)
        {
            var assetRequest = await _context.AssetRequests.FindAsync(id);
            if (assetRequest == null)
            {
                throw new AssetRequestNotFoundException($"Request with ID {id} Not Found");
            }

            _context.AssetRequests.Remove(assetRequest);

        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }

}

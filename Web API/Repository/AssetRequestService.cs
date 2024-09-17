using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;
using Hexa_Hub.Exceptions;
using Hexa_Hub.DTO;
using static Hexa_Hub.Models.MultiValues;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Hexa_Hub.Repository
{
    public class AssetRequestService : IAssetRequest
    {
        private readonly DataContext _context;
        private readonly IAssetAllocation _assetAlloc;
        private readonly IAsset _asset;

        public AssetRequestService(DataContext context, IAssetAllocation assetAlloc, IAsset asset)
        {
            _context = context;
            _assetAlloc = assetAlloc;
            _asset = asset;
        }

        public async Task<List<AssetRequest>> GetAllAssetRequests()
        {
            return await _context.AssetRequests
                .Include(ar=>ar.Asset)
                .Include(ar=>ar.User)
                .ToListAsync();
        }
        public async Task<List<AssetRequest>> GetAssetRequestByMonthAsync(string month)
        {
            var monthname = DateTime.ParseExact(month, "MMMM", null).Month;
            return await _context.AssetRequests
                                 .Where(a => a.AssetReqDate.Month == monthname)
                                 .ToListAsync();
        }

        public async Task<List<AssetRequest>> GetAssetRequestByYearAsync(int year)
        {
            return await _context.AssetRequests
                                 .Where(a => a.AssetReqDate.Year == year)
                                 .ToListAsync();
        }

        public async Task<List<AssetRequest>> GetAssetRequestByMonthAndYearAsync(string month, int year)
        {
            var monthname = DateTime.ParseExact(month, "MMMM", null).Month;
            return await _context.AssetRequests
                                 .Where(a => a.AssetReqDate.Month == monthname && a.AssetReqDate.Year == year)
                                 .ToListAsync();
        }

        public async Task<List<AssetRequest>> GetAssetRequestByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.AssetRequests
                                 .Where(a => a.AssetReqDate>= startDate && a.AssetReqDate <= endDate)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<AssetRequestDto>> GetAssetRequestByStatus(RequestStatus status)
        {
            var reqByStatus = await (from request in _context.AssetRequests
                                        where request.Request_Status == status
                                        select new AssetRequestDto
                                        {
                                            AssetReqId = request.AssetReqId,
                                            UserId = request.UserId,
                                            AssetId = request.AssetId,
                                            CategoryId = request.CategoryId,
                                            AssetReqDate = request.AssetReqDate,
                                            AssetReqReason= request.AssetReqReason, 
                                            Request_Status = request.Request_Status.ToString()
                                           
                                        }).ToListAsync();

            return reqByStatus;
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

        public async Task<AssetRequest> UpdateAssetRequest(int id, AssetRequestDto assetRequestDto)
        {
            var existingRequest = await GetAssetRequestById(id);
            if (existingRequest == null)
            {
                throw new AssetRequestNotFoundException($"Asset request with ID {id} not found.");
            }

            // Check if the status has changed
            if (assetRequestDto.Request_Status != existingRequest.Request_Status.ToString())
            {
                // Parse the status from the DTO to the enum
                if (Enum.TryParse(assetRequestDto.Request_Status, out RequestStatus parsedStatus))
                {
                    existingRequest.Request_Status = parsedStatus;

                    // If status changes to Allocated, handle the asset allocation logic
                    if (parsedStatus == RequestStatus.Allocated)
                    {
                        var existingAllocId = await _context.AssetAllocations
                            .FirstOrDefaultAsync(aa => aa.AssetReqId == assetRequestDto.AssetReqId);

                        if (existingAllocId == null)
                        {
                            // Create new asset allocation
                            var assetAllocation = new AssetAllocation
                            {
                                AssetId = assetRequestDto.AssetId,
                                UserId = assetRequestDto.UserId,
                                AssetReqId = assetRequestDto.AssetReqId,
                                AllocatedDate = DateTime.Now
                            };
                            await _assetAlloc.AddAllocation(assetAllocation);

                            // Update the asset status to Allocated
                            var asset = await _asset.GetAssetById(assetRequestDto.AssetId);
                            if (asset != null)
                            {
                                asset.Asset_Status = AssetStatus.Allocated;
                                _asset.UpdateAsset(asset);
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid Request Status provided.");
                }
            }

            // Save changes to the context
            await _context.SaveChangesAsync();
            return existingRequest;
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

using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;
using Hexa_Hub.Exceptions;

namespace Hexa_Hub.Repository
{
    public class ReturnRequestRepo : IReturnReqRepo
    {
        private readonly DataContext _context;
        public ReturnRequestRepo(DataContext context)
        {
            _context = context;
        }
        public async Task AddReturnRequest(ReturnRequest returnRequest)
        {
             _context.ReturnRequests.AddAsync(returnRequest);
        }

        public async Task DeleteReturnRequest(int id)
        {
            var req = await _context.ReturnRequests.FindAsync(id);
            if (req == null)
                throw new ReturnRequestNotFoundException($"Return Request with ID {id} Not Found");
            _context.ReturnRequests.Remove(req);
        }

        public async Task<List<ReturnRequest>> GetAllReturnRequest()
        {
            return await _context.ReturnRequests
                .Include(rr => rr.Asset)
                .Include(rr => rr.User)
                .ToListAsync();
        }

        public async Task<ReturnRequest?> GetReturnRequestById(int id)
        {
            return await _context.ReturnRequests
                .Include(rr => rr.Asset)
                .Include(rr => rr.User)
                .FirstOrDefaultAsync(rr => rr.ReturnId == id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateReturnRequest(ReturnRequest returnRequest)
        {
           
            _context.Attach(returnRequest);
            _context.Entry(returnRequest).State = EntityState.Modified;
        }

        public async Task<List<ReturnRequest>> GetReturnRequestsByUserId(int userId)
        {
            return await _context.ReturnRequests
                .Where(rr => rr.UserId == userId)
                .Include(rr => rr.Asset)
                .Include(rr => rr.User)
                .ToListAsync();
        }

        public async Task<bool> UserHasAsset(int id)
        {
            return await _context.AssetAllocations
                 .AnyAsync(aa => aa.UserId == id);
        }
    }
}

using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;

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
                throw new Exception("Id Not Found");
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

        public Task<ReturnRequest> UpdateReturnRequest(ReturnRequest returnRequest)
        {
            _context.ReturnRequests.Update(returnRequest);
            return Task.FromResult(returnRequest);
        }
    }
}

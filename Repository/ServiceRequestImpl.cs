using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Repository
{
    public class ServiceRequestImpl : IServiceRequest
    {
        private readonly DataContext _context;

        public ServiceRequestImpl(DataContext context)
        {
            _context = context;
        }

        public async Task<List<ServiceRequest>> GetAllServiceRequests()
        {
            return await _context.ServiceRequests
                                 .Include(sr => sr.Asset)
                                 .Include(sr => sr.User)
                                 .ToListAsync();
        }

        public async Task<ServiceRequest?> GetServiceRequestById(int id)
        {
            return await _context.ServiceRequests
                                 .Include(sr => sr.Asset)
                                 .Include(sr => sr.User)
                                 .FirstOrDefaultAsync(sr => sr.ServiceId == id);
        }

        public async Task<ServiceRequest> AddServiceRequest(ServiceRequest serviceRequest)
        {
            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();
            return serviceRequest;
        }

        public async Task<ServiceRequest> UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            _context.ServiceRequests.Update(serviceRequest);
            await _context.SaveChangesAsync();
            return serviceRequest;
        }

        public async Task<bool> DeleteServiceRequest(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null)
            {
                return false;
            }

            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}

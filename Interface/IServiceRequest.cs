namespace Hexa_Hub.Interface
{
    public interface IServiceRequest
    {
        Task<List<ServiceRequest>> GetAllServiceRequests();
        Task<ServiceRequest?> GetServiceRequestById(int id);
        Task<ServiceRequest> AddServiceRequest(ServiceRequest serviceRequest);
        Task<ServiceRequest> UpdateServiceRequest(ServiceRequest serviceRequest);
        Task<bool> DeleteServiceRequest(int id);
    }

}

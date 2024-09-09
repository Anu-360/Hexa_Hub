namespace Hexa_Hub.Interface
{
    public interface IReturnReqRepo
    {
        Task<List<ReturnRequest>> GetAllReturnRequest();
        Task<ReturnRequest?> GetReturnRequestById(int id);
        Task AddReturnRequest(ReturnRequest returnRequest);
        Task<ReturnRequest> UpdateReturnRequest(ReturnRequest returnRequest);
        Task DeleteReturnRequest(int id);
        Task Save();
    }
}

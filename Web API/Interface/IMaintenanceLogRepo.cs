using Hexa_Hub.DTO;

namespace Hexa_Hub.Interface
{
    public interface IMaintenanceLogRepo
    {
        Task<List<MaintenanceLog>> GetAllMaintenanceLog();
        
        Task<byte[]> GenerateMaintenanceInvoicePdfAsync(int maintenanceId);
        Task<List<MaintenanceLog>> GetMaintenanceLogById(int userId);
        Task AddMaintenanceLog(MaintenanceLog maintenanceLog);
       
        Task<bool> UpdateMaintenanceLog(int id, MaintenanceDto maintenanceDto);
        Task DeleteMaintenanceLog(int id);
        Task Save();

        Task<List<MaintenanceLog>> GetMaintenanceLogByUserId(int userId);
    }
}

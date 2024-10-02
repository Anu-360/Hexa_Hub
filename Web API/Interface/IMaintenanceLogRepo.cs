using Hexa_Hub.DTO;

namespace Hexa_Hub.Interface
{
    public interface IMaintenanceLogRepo
    {
        Task<List<MaintenanceClassDto>> GetAllLog();
        Task<List<MaintenanceLog>> GetAllMaintenanceLog();
        Task<MaintenanceClassDto> GetMaintenanceById(int id);
        Task<byte[]> GenerateMaintenanceInvoicePdfAsync(int maintenanceId);
        Task<List<MaintenanceLog>> GetMaintenanceLogById(int userId);
        Task AddMaintenanceLog(MaintenanceLog maintenanceLog);

        //Task<bool> UpdateMaintenanceLog(int id, MaintenanceDto maintenanceDto);
        Task<bool> UpdateMaintenanceLog(MaintenanceClassDto maintenanceClassDto);
        Task DeleteMaintenanceLog(int id);
        Task Save();

        Task<List<MaintenanceLog>> GetMaintenanceLogByUserId(int userId);
    }
}

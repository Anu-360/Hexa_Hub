using Hexa_Hub.DTO;

namespace Hexa_Hub.Interface
{
    public interface IMaintenanceLogRepo
    {
        Task<List<MaintenanceLog>> GetAllMaintenanceLog();
        //Task<MaintenanceLog?> GetMaintenanceLogById(int id);
        Task<List<MaintenanceLog>> GetMaintenanceLogById(int userId);
        Task AddMaintenanceLog(MaintenanceLog maintenanceLog);
        //Task<MaintenanceLog> UpdateMaintenanceLog(MaintenanceLog maintenanceLog);
        Task<bool> UpdateMaintenanceLog(int id, MaintenanceDto maintenanceDto);
        Task DeleteMaintenanceLog(int id);
        Task Save();

        Task<List<MaintenanceLog>> GetMaintenanceLogByUserId(int userId);
    }
}

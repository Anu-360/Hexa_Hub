using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;
using Hexa_Hub.Exceptions;
using Hexa_Hub.DTO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;


namespace Hexa_Hub.Repository
{
    public class MaintenanceLogRepo : IMaintenanceLogRepo
    {
        private readonly DataContext _context;

        public MaintenanceLogRepo(DataContext context)
        {
            _context = context;
        }
        public async Task<List<MaintenanceClassDto>> GetAllLog()
        {
            return await _context.MaintenanceLogs
                .Include(ml => ml.Asset)
                .Include(ml => ml.User)
                .Select(ml => new MaintenanceClassDto
                {
                    MaintenanceId = ml.MaintenanceId,
                    AssetId = ml.Asset.AssetId,
                    AssetName = ml.Asset.AssetName,
                    UserId = ml.User.UserId,
                    UserName = ml.User.UserName,
                    Maintenance_date = ml.Maintenance_date,
                    Cost = ml.Cost,
                    Maintenance_Description = ml.Maintenance_Description,
                })
                .OrderByDescending(ml=>ml.Maintenance_date)
                .ToListAsync();
        }

        public async Task<List<MaintenanceLog>> GetAllMaintenanceLog()
        {
            return await _context.MaintenanceLogs
                .Include(ml => ml.Asset)
                .Include(ml => ml.User)
                .ToListAsync();
        }
        public async Task<byte[]> GenerateMaintenanceInvoicePdfAsync(int maintenanceId)
        {
            try
            {
                var maintenanceLog = await _context.MaintenanceLogs
                    .Include(ml => ml.Asset)
                    .Include(ml => ml.User)
                    .FirstOrDefaultAsync(ml => ml.MaintenanceId == maintenanceId);

                if (maintenanceLog == null)
                {
                    throw new Exception("Maintenance log not found.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    var writer = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(writer);
                    var document = new Document(pdf);

                    document.Add(new Paragraph("Maintenance Invoice")
                        .SetFontSize(20)
                        .SetBold()
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    document.Add(new Paragraph($"Maintenance ID: {maintenanceLog.MaintenanceId}"));
                    document.Add(new Paragraph($"Asset ID: {maintenanceLog.AssetId}"));
                    document.Add(new Paragraph($"Asset Name: {maintenanceLog.Asset?.AssetName}"));
                    document.Add(new Paragraph($"User ID: {maintenanceLog.UserId}"));
                    document.Add(new Paragraph($"User Name: {maintenanceLog.User?.UserName}"));
                    document.Add(new Paragraph($"Maintenance Date: {maintenanceLog.Maintenance_date:yyyy-MM-dd}"));
                    document.Add(new Paragraph($"Cost: {maintenanceLog.Cost:C}"));
                    document.Add(new Paragraph($"Description: {maintenanceLog.Maintenance_Description}"));

                    document.Close();
                    return memoryStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                // Log exception here
                throw new Exception("An error occurred while generating the PDF: " + ex.Message);
            }
        }

        public async Task<MaintenanceClassDto> GetMaintenanceById(int id)
        {
            return await _context.MaintenanceLogs
                .Include(ml => ml.Asset)
                .Include(ml => ml.User)
                .Select(ml=> new MaintenanceClassDto
                {
                    MaintenanceId = ml.MaintenanceId,
                    AssetId = ml.Asset.AssetId,
                    AssetName = ml.Asset.AssetName,
                    UserId = ml.User.UserId,
                    UserName = ml.User.UserName,
                    Maintenance_date = ml.Maintenance_date,
                    Cost = ml.Cost,
                    Maintenance_Description = ml.Maintenance_Description,
                })
                .FirstOrDefaultAsync(ml => ml.MaintenanceId == id);
        }

        public async Task<List<MaintenanceLog>> GetMaintenanceLogById(int userId)
        {
            return await _context.MaintenanceLogs
                .Include(ml => ml.Asset)
                .Include(ml => ml.User)
                .Where(ml => ml.UserId == userId)
                .ToListAsync();
        }

        public async Task AddMaintenanceLog(MaintenanceLog maintenanceLog)
        {
            _context.MaintenanceLogs.AddAsync(maintenanceLog);
        }

        public async Task DeleteMaintenanceLog(int id)
        {
            var log = await _context.MaintenanceLogs.FindAsync(id);
            if (log == null)
            {
                throw new MaintenanceLogNotFoundException($"Maintenance Log with ID {id} Not Found");
            }
            _context.MaintenanceLogs.Remove(log);

        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        //public async Task<bool> UpdateMaintenanceLog(int id, MaintenanceDto maintenanceDto)
        //{
        //    var existingLog = await _context.MaintenanceLogs.FindAsync(id);
        //    if (existingLog == null)
        //    {
        //        return false;
        //    }

        //    existingLog.Maintenance_date = maintenanceDto.Maintenance_date;
        //    existingLog.Cost = maintenanceDto.Cost;
        //    existingLog.Maintenance_Description = maintenanceDto.Maintenance_Description;

        //    _context.MaintenanceLogs.Update(existingLog);

        //    return true;
        //}

        public async Task<bool> UpdateMaintenanceLog(MaintenanceClassDto maintenanceClassDto)
        {
            var existingLog = await _context.MaintenanceLogs.FindAsync(maintenanceClassDto.MaintenanceId);
            if (existingLog == null)
            {
                return false;
            }

            existingLog.Maintenance_date = maintenanceClassDto.Maintenance_date;
            existingLog.Cost = maintenanceClassDto.Cost;
            existingLog.Maintenance_Description = maintenanceClassDto.Maintenance_Description;

            _context.MaintenanceLogs.Update(existingLog);
            return true;
        }




        public async Task<List<MaintenanceLog>> GetMaintenanceLogByUserId(int userId)
        {
            return await _context.MaintenanceLogs
                .Where(ml => ml.UserId == userId)
                .Include(ml => ml.Asset)
                .Include(ml => ml.User)
                .ToListAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;

using Hexa_Hub.Interface;
using Hexa_Hub.Repository;
namespace Hexa_Hub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddScoped<ICategory, CategoryService>();
            builder.Services.AddScoped<IAsset, AssetService>(); 
            builder.Services.AddScoped<IAssetAllocation, AssetAllocationService>(); 
            builder.Services.AddScoped<IAssetRequest, AssetRequestService>();   
            builder.Services.AddScoped<ISubCategory,SubCategoryService>();  
            builder.Services.AddScoped<IServiceRequest, ServiceRequestImpl>();
            builder.Services.AddScoped<IAuditRepo,AuditRepo>();
            builder.Services.AddScoped<IMaintenanceLogRepo, MaintenanceLogRepo>();
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<IReturnReqRepo, ReturnRequestRepo>();
            builder.Services.AddScoped<IUserProfileRepo,UserProfileRepo>();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using Hexa_Hub.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Hexa_Hub.Exceptions;
using Hexa_Hub.DTO;
using System.Text;

namespace Hexa_Hub.Repository
{
    public class AssetService : IAsset
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _environment;

        public AssetService(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<List<Asset>> GetAllAssets()
        {
            return await _context.Assets
                                 .Include(a => a.Category)
                                 .Include(a => a.SubCategories)
                                 .ToListAsync();
        }
        public async Task<List<Asset>> GetAllDetailsOfAssets()
        {
            return await _context.Assets
                                 .Include(a => a.Category)
                                 .Include(a => a.SubCategories)
                                 .Include(a => a.AssetRequests)
                                 .Include(a => a.ServiceRequests)
                                 .Include(a => a.MaintenanceLogs)
                                 .Include(a => a.Audits)
                                 .Include(a => a.ReturnRequests)
                                 .Include(a => a.AssetAllocations)
                                 .ToListAsync();
        }

        public async Task<Asset?> GetAssetById(int id)
        {
            return await _context.Assets
                                 .Include(a => a.Category)
                                 .Include(a => a.SubCategories)
                                 .Include(a => a.AssetRequests)
                                 .Include(a => a.ServiceRequests)
                                 .Include(a => a.MaintenanceLogs)
                                 .Include(a => a.Audits)
                                 .Include(a => a.ReturnRequests)
                                 .Include(a => a.AssetAllocations)
                                 .FirstOrDefaultAsync(a => a.AssetId == id);
        }

        //public async Task AddAsset(Asset asset)
        //{
        //    if(asset.AssetImage == null)
        //    {
        //        string defaultImagePath = GetDefaultAssetImagePath();
        //        asset.AssetImage = await GetImageBytesAsync(defaultImagePath);
        //    }

        //    await _context.AddAsync(asset);
        //}
        public async Task<Asset> AddAsset(AssetDto assetDto)
        {
            var asset = new Asset
            {
                AssetId = assetDto.AssetId,
                AssetName = assetDto.AssetName,
                AssetDescription = assetDto.AssetDescription,
                CategoryId = assetDto.CategoryId,
                SubCategoryId = assetDto.SubCategoryId,
                AssetImage = assetDto.AssetImage,
                SerialNumber = assetDto.SerialNumber,
                Model = assetDto.Model,
                ManufacturingDate = assetDto.ManufacturingDate,
                Location = assetDto.Location,
                Value = assetDto.Value,
                Expiry_Date = assetDto.Expiry_Date
            };
            await _context.AddAsync(asset);
            await _context.SaveChangesAsync();

            //const string defaultImg = "AssetDefault.jpg";
            //asset.AssetImage = Encoding.UTF8.GetBytes(defaultImg);
            const string defaultImageFileName = "profile-img.jpg";
            const string imagesFolder = "Images";
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), imagesFolder);
            string defaultImagePath = Path.Combine(imagePath, defaultImageFileName);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            if (!File.Exists(defaultImagePath))
            {
                string sourcePath = GetDefaultAssetImagePath();
                if (!File.Exists(sourcePath))
                {
                    throw new FileNotFoundException("Source default image file not found.", sourcePath);
                }
                File.Copy(sourcePath, defaultImagePath);
            }
            asset.AssetImage = Encoding.UTF8.GetBytes(defaultImageFileName);
            _context.Assets.Update(asset);
            await _context.SaveChangesAsync();

            return asset;
        }

        public async Task<Asset> UpdateAssetDto(int id, AssetDto assetDto)
        {
            var existingAsset = await _context.Assets.FindAsync(id);
            if (existingAsset == null)
            {
                throw new AssetNotFoundException($"Asset with ID {id} not found");
            }

            existingAsset.AssetName = assetDto.AssetName;
            existingAsset.AssetDescription = assetDto.AssetDescription;
            existingAsset.CategoryId = assetDto.CategoryId;
            existingAsset.SubCategoryId = assetDto.SubCategoryId;
            existingAsset.AssetImage = assetDto.AssetImage ?? existingAsset.AssetImage;
            existingAsset.SerialNumber = assetDto.SerialNumber;
            existingAsset.Model = assetDto.Model;
            existingAsset.ManufacturingDate = assetDto.ManufacturingDate;
            existingAsset.Location = assetDto.Location;
            existingAsset.Value = assetDto.Value;
            existingAsset.Expiry_Date = assetDto.Expiry_Date;


            _context.Assets.Update(existingAsset);
            return existingAsset;
        }
        public async Task<Asset> UpdateAsset(Asset asset)
        {
            _context.Assets.Update(asset);
            return asset;
        }

        public async Task DeleteAsset(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
            throw new AssetNotFoundException($"Asset with ID {id} Not Found");
            }

            _context.Assets.Remove(asset);

        }
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<string?> UploadAssetImageAsync(int assetId, IFormFile file)
        {
            var asset = await _context.Assets.FindAsync(assetId);
            if(asset == null)
            {
                return null;
            }
            const string assetImageFolder = "AssetImages";
            string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), assetImageFolder);
            if (!Directory.Exists(ImagePath))
            {
                Directory.CreateDirectory(ImagePath);
            }
            string fileName;
            if (asset.AssetImage == null && file == null)
            {
                fileName = "AssetDefault.jpg";
            }
            else if(file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                fileName = $"{assetId}{fileExtension}";
                string fullPath = Path.Combine(ImagePath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            else
            {
                return Encoding.UTF8.GetString(asset.AssetImage);
            }
            asset.AssetImage = Encoding.UTF8.GetBytes(fileName);
            await _context.SaveChangesAsync();
            return fileName;
        }

        private string GetDefaultAssetImagePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "AssetImages", "AssetDefault.jpg");
        }

        public string GetImagePath(string fileName)
        {
            return Path.Combine("AssetImages", fileName);
        }




        //public async Task<string?> UploadAssetImageAsync(int assetId, IFormFile file)
        //{
        //    var assetData = await _context.Assets.FindAsync(assetId);
        //    if(assetData == null) 
        //    { 
        //        return null; 
        //    }
        //    string imagePath = Path.Combine(_environment.ContentRootPath, "AssetImages");
        //    if (!Directory.Exists(imagePath))
        //    {
        //        Directory.CreateDirectory(imagePath);
        //    }
        //    if (assetData.AssetImage == null && file == null)
        //    {
        //        string defaultImagePath = GetDefaultAssetImagePath();
        //        assetData.AssetImage = await GetImageBytesAsync(defaultImagePath);
        //    }
        //    else if(file != null)
        //    {
        //        string fileName = $"{assetId}_{Path.GetFileName(file.FileName)}";
        //        string fullPath = Path.GetFullPath(imagePath, fileName);

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }
        //        assetData.AssetImage = await File.ReadAllBytesAsync(fullPath);
        //    }
        //    await _context.SaveChangesAsync();
        //    return file?.FileName ?? "AssetDefault.jpg";
        //}

        //public string GetDefaultAssetImagePath()
        //{
        //    return Path.Combine(_environment.ContentRootPath, "AssetImages", "AssetDefault.jpg");
        //}
        //private async Task<byte[]> GetImageBytesAsync(string path)
        //{
        //    return await File.ReadAllBytesAsync(path);
        //}
    }

}

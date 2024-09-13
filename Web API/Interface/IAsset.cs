using Hexa_Hub.DTO;

namespace Hexa_Hub.Interface
{
    public interface IAsset
    {
        Task<List<Asset>> GetAllAssets();
        Task<Asset?> GetAssetById(int id);
        Task<List<Asset>> GetAllDetailsOfAssets();
        //Task AddAsset(Asset asset);
        Task<Asset> AddAsset(AssetDto assetDto);
        Task<Asset> UpdateAsset(Asset asset);
        Task<Asset> UpdateAssetDto(int id, AssetDto assetDto);
        Task DeleteAsset(int id);
        Task Save();
        Task<string?> UploadAssetImageAsync(int assetId, IFormFile file);
        public string GetImagePath(string fileName);
    }

}

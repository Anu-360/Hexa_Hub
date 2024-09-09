namespace Hexa_Hub.Interface
{
    public interface IAsset
    {
        Task<List<Asset>> GetAllAssets();
        Task<Asset?> GetAssetById(int id);
        Task<Asset> AddAsset(Asset asset);
        Task<Asset> UpdateAsset(Asset asset);
        Task<bool> DeleteAsset(int id);
    }

}

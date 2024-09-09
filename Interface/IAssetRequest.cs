namespace Hexa_Hub.Interface
{
    public interface IAssetRequest
    {
        Task<List<AssetRequest>> GetAllAssetRequests();
        Task<AssetRequest?> GetAssetRequestById(int id);
        Task<AssetRequest> AddAssetRequest(AssetRequest assetRequest);
        Task<AssetRequest> UpdateAssetRequest(AssetRequest assetRequest);
        Task<bool> DeleteAssetRequest(int id);
    }

}

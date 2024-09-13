using Hexa_Hub.DTO;

namespace Hexa_Hub.Interface
{
    public interface IAssetRequest
    {
        Task<List<AssetRequest>> GetAllAssetRequests();
        Task<AssetRequest?> GetAssetRequestById(int id);
        Task AddAssetRequest(AssetRequestDto dto);
        Task<AssetRequest> UpdateAssetRequest(AssetRequest assetRequest);
        Task DeleteAssetRequest(int id);
        Task Save();
        Task<List<AssetRequest>> GetAssetRequestsByUserId(int userId);
    }

}

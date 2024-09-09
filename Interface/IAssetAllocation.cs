namespace Hexa_Hub.Interface
{
    public interface IAssetAllocation
    {
        Task<List<AssetAllocation>> GetAllAllocations();
        Task<AssetAllocation?> GetAllocationById(int id);
        Task<AssetAllocation> AddAllocation(AssetAllocation allocation);
        Task<AssetAllocation> UpdateAllocation(AssetAllocation allocation);
        Task<bool> DeleteAllocation(int id);
    }

}

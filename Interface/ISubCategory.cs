namespace Hexa_Hub.Interface
{
    public interface ISubCategory
    {
        Task<List<SubCategory>> GetAllSubCategories();
        Task<SubCategory?> GetSubCategoryById(int id);
        Task<SubCategory> AddSubCategory(SubCategory subcategory);
        Task<SubCategory> UpdateSubCategory(SubCategory subcategory);
        Task<bool> DeleteSubCategory(int id);

        Task<bool> SubCategoryExists(int subCategoryId);
    }
}

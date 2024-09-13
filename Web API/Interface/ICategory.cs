using Hexa_Hub.DTO;
using Microsoft.EntityFrameworkCore;
namespace Hexa_Hub.Interface
{

    public interface ICategory
    {
        Task<List<Category>> GetAllCategories();
        Task<Category> AddCategory(CategoriesDto category);
        //Task<Category> AddCategory(Category category);
        //Task<Category> UpdateCategory(Category category);
        Task<bool> UpdateCategoryAsync(int id, CategoriesDto categoryDto);
        Task DeleteCategory(int id);
        Task Save();
    }

}


using Hexa_Hub.Repository;
using Microsoft.EntityFrameworkCore;
namespace Hexa_Hub.Repository
{
   
        public interface ICategory
        {
            Task<List<Category>> GetAllCategories();
            Task<Category?> GetCategoryById(int id);
            Task<Category> AddCategory(Category category);
            Task<Category> UpdateCategory(Category category);
            Task<bool> DeleteCategory(int id);
        }

}


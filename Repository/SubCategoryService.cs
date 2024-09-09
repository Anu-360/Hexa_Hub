using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Repository
{
    public class SubCategoryService:ISubCategory
    {
        private readonly DataContext _context;

        public SubCategoryService(DataContext context)
        {
            _context = context; 
        }

        public async Task<List<SubCategory>> GetAllSubCategories()
        {
            return await _context.SubCategories.Include(c => c.Assets).Include(c => c.Category).ToListAsync();
        }


        public async Task<SubCategory?> GetSubCategoryById(int subCategoryId)
        {
            return await _context.SubCategories
                .Include(sc => sc.Category)
                .Include(sc => sc.Assets)
                .FirstOrDefaultAsync(sc => sc.SubCategoryId == subCategoryId);
        }

        public async Task<SubCategory> AddSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Add(subCategory);
            await _context.SaveChangesAsync();
            return subCategory; 
        }

        public async Task<SubCategory> UpdateSubCategory(SubCategory subCategory)
        {
            _context.SubCategories.Update(subCategory);
            await _context.SaveChangesAsync();
            return subCategory; 
        }

        public async Task<bool> DeleteSubCategory(int subCategoryId)
        {
            var subCategory = await GetSubCategoryById(subCategoryId);
            if (subCategory != null)
            {
                _context.SubCategories.Remove(subCategory);
                await _context.SaveChangesAsync();
            }
            return false;
        }

        public async Task<bool> SubCategoryExists(int subCategoryId)
        {
            return await _context.SubCategories.AnyAsync(sc => sc.SubCategoryId == subCategoryId);
        }
    }
}

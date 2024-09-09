using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Repository
{
    public class UserProfileRepo : IUserProfileRepo
    {
        private readonly DataContext _context;
        public UserProfileRepo(DataContext context)
        {
            _context = context;
        }
        public async Task AddProfiles(UserProfile userProfile)
        {
            _context.AddAsync(userProfile);
        }

        public async Task DeleteProfiles(int id)
        {
            var profile = await _context.UserProfiles.FindAsync(id);
            if (profile == null)
            {
                throw new Exception("Profile Not Found");
            }
            _context.UserProfiles.Update(profile);
        }

        public async Task<List<UserProfile>> GetAllProfiles()
        {
            return await _context.UserProfiles
                .Include(up=>up.User)
                .ToListAsync();
        }

        public async Task<UserProfile?> GetProfilesById(int id)
        {
            return await _context.UserProfiles
                .Include(up=>up.User)
                .FirstOrDefaultAsync(up=>up.UserId == id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public Task<UserProfile> UpdateProfiles(UserProfile userProfile)
        {
            _context.UserProfiles.Update(userProfile);
            return Task.FromResult(userProfile);
        }
    }
}

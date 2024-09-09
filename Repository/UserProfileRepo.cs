using Hexa_Hub.Interface;
using Microsoft.EntityFrameworkCore;

namespace Hexa_Hub.Repository
{
    public class UserProfileRepo : IUserProfileRepo
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _environment;
        public UserProfileRepo(DataContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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

        //public async Task<List<UserProfile>> GetAllProfiles()
        //{
        //    return await _context.UserProfiles
        //        .Include(up=>up.User)
        //        .ToListAsync();
        //}

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

        //public Task<UserProfile> UpdateProfiles(UserProfile userProfile)
        //{
        //    _context.UserProfiles.Update(userProfile);
        //    return Task.FromResult(userProfile);
        //}
        //public async Task<string?> UploadProfileImageAsync(int userId, IFormFile file)
        //{
        //    var userProfile = await _context.UserProfiles.FindAsync(userId);
        //    if (userProfile == null) return null;

        //    string imagePath = Path.Combine(_environment.ContentRootPath, "images");

        //    if (!Directory.Exists(imagePath))
        //    {
        //        Directory.CreateDirectory(imagePath);
        //    }

        //    // Set a default image if the profile image is null
        //    if (userProfile.ProfileImage == null && file == null)
        //    {
        //        string defaultImagePath = Path.Combine(imagePath, "default.png");
        //        userProfile.ProfileImage = await File.ReadAllBytesAsync(defaultImagePath);
        //    }
        //    else if (file != null)
        //    {
        //        string fileName = $"{userId}_{Path.GetFileName(file.FileName)}";
        //        string fullPath = Path.Combine(imagePath, fileName);

        //        using (var stream = new FileStream(fullPath, FileMode.Create))
        //        {
        //            await file.CopyToAsync(stream);
        //        }

        //        userProfile.ProfileImage = await File.ReadAllBytesAsync(fullPath);
        //    }

        //    await _context.SaveChangesAsync();
        //    return file?.FileName ?? "default.png";
        //}

        //public async Task<string?> GetProfileImageAsync(int userId)
        //{
        //    var userProfile = await _context.UserProfiles.FindAsync(userId);
        //    if (userProfile?.ProfileImage == null) return null;

        //    string imagePath = Path.Combine(_environment.ContentRootPath, "images");

        //    string imageName = userProfile.ProfileImage == await File.ReadAllBytesAsync(Path.Combine(imagePath, "default.png"))
        //        ? "default.png"
        //        : $"{userId}.png";

        //    return Path.Combine("images", imageName);
        //}

        //public async Task<bool> SetDefaultProfileImageAsync(int userId)
        //{
        //    var userProfile = await _context.UserProfiles.FindAsync(userId);
        //    if (userProfile == null) return false;

        //    string imagePath = Path.Combine(_environment.ContentRootPath, "images");
        //    string defaultImagePath = Path.Combine(imagePath, "default.png");

        //    if (!File.Exists(defaultImagePath))
        //    {
        //        return false; // Default image file does not exist
        //    }

        //    userProfile.ProfileImage = await File.ReadAllBytesAsync(defaultImagePath);

        //    await _context.SaveChangesAsync();
        //    return true;
        //}
    }
}

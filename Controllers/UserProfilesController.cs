using Hexa_Hub.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Hexa_Hub.Models.MultiValues;

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfilesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly IUserProfileRepo _userProfile;

        public UserProfilesController(DataContext context, IUserProfileRepo userProfileRepo, IWebHostEnvironment environment)
        {
            _context = context;
            _userProfile = userProfileRepo;
            _environment = environment;
        }

        //// GET: api/UserProfiles
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfiles()
        //{
        //    //return await _context.UserProfiles.ToListAsync();
        //    return await _userProfile.GetAllProfiles();
        //}

        // GET: api/UserProfiles/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(int id)
        {
            //var userProfile = await _context.UserProfiles.FindAsync(id);
            var userProfile = await _userProfile.GetProfilesById(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return userProfile;
        }

        // PUT: api/UserProfiles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUserProfile(int id, UserProfile userProfile, UserType newRole)
        {
            if (id != userProfile.UserId)
            {
                return BadRequest();
            }
            var existingProfile = await _userProfile.GetProfilesById(id);
            if (existingProfile == null)
            {
                return NotFound("UserProfile not found.");
            }

            existingProfile.UserName = userProfile.UserName;
            existingProfile.UserMail = userProfile.UserMail;
            existingProfile.Gender = userProfile.Gender;
            existingProfile.Dept = userProfile.Dept;
            existingProfile.Designation = userProfile.Designation;
            existingProfile.PhoneNumber = userProfile.PhoneNumber;
            existingProfile.Address = userProfile.Address;

            var user = existingProfile.User;
            if (user == null)
            {
                return NotFound("Associated User not found.");
            }

            if (user.User_Type != newRole)
            {
                user.User_Type = newRole;
            }
            
            try
            {
                await _userProfile.UpdateProfiles(existingProfile);
                await _userProfile.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        //[HttpPost("{userId}/UplaodImage")]
        ////public async Task<IActionResult> UploadProfileImage(int userId, IFormFile)

        //// POST: api/UserProfiles
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<UserProfile>> RegisterUserProfile(UserProfile userProfile)
        //{
        //    _context.UserProfiles.Add(userProfile);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (UserProfileExists(userProfile.UserId))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetUserProfile", new { id = userProfile.UserId }, userProfile);
        //}

        //// DELETE: api/UserProfiles/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserProfile(int id)
        //{
        //    var userProfile = await _context.UserProfiles.FindAsync(id);
        //    if (userProfile == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UserProfiles.Remove(userProfile);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(e => e.UserId == id);
        }

        [HttpPut("{userId}/upload")]
        public async Task<IActionResult> UploadProfileImage(int userId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileName = await _userProfile.UploadProfileImageAsync(userId, file);
            if (fileName == null)
            {
                return NotFound();
            }

            return Ok(new { FileName = fileName });
        }

    }
}

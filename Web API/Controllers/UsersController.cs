using Hexa_Hub.DTO;
using Hexa_Hub.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace Hexa_Hub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        private readonly DataContext _context;

        public UsersController(DataContext context, IUserRepo userRepo)
        {
            _context = context;
            _userRepo = userRepo;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userRepo.GetAllUser();
        }


        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<User>> GetUserProfile()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (int.TryParse(currentUserId, out int userId))
            {
                var user = await _userRepo.GetUserById(userId);
                if (user == null)
                {
                    return NotFound();
                }
                var userDto = new UserDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    UserMail = user.UserMail,
                    Gender = user.Gender,
                    Dept = user.Dept,
                    Designation = user.Designation,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                    Branch = user.Branch,
                    User_Type = user.User_Type,
                    ProfileImage = user.ProfileImage
                };

                return Ok(userDto);
            }

            return BadRequest("Invalid user ID.");
        }




        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Employee,Admin")]
        public async Task<IActionResult> UpdateUserDetails(int id, UserDto userDto)
        {
            if (id != userDto.UserId)
            {
                return BadRequest();
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id.ToString() != currentUserId)
            {
                return Forbid();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.UserName = userDto.UserName;
            user.UserMail = userDto.UserMail;
            user.Gender = userDto.Gender;
            user.Dept = userDto.Dept;
            user.Designation = userDto.Designation;
            user.PhoneNumber = userDto.PhoneNumber;
            user.Address = userDto.Address;
            user.Branch = userDto.Branch;
            user.ProfileImage = userDto.ProfileImage;

            if (User.IsInRole("Admin"))
            {
                user.User_Type = userDto.User_Type;
            }
            else
            {
                user.User_Type = user.User_Type;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        [HttpPatch("{id}/password")]
        [Authorize]
        public async Task<IActionResult> ChangeUserPassword(int id, PasswordDto passwordChangeDto)
        {
            if (id != passwordChangeDto.UserId)
            {
                return BadRequest();
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id.ToString() != currentUserId)
            {
                return Forbid();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Password != passwordChangeDto.CurrentPassword)
            {
                return Unauthorized("Current password is incorrect.");
            }

            user.Password = passwordChangeDto.NewPassword;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        //// PUT: api/Users/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //[Authorize]
        //public async Task<IActionResult> PutUser(int id, User user)
        //{
        //    var userRole = User.FindFirstValue(ClaimTypes.Role);
        //    if (id != user.UserId)
        //    {
        //        return BadRequest();
        //    }
        //    if (userRole != "Admin")
        //    {
        //        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //        if (id != userId)
        //        {
        //            return Forbid();
        //        }
        //    }

        //    _userRepo.UpdateUser(user);

        //    var userProfile = await _userProfileRepo.GetProfilesById(id);
        //    if (userProfile != null)
        //    {
        //        userProfile.UserName = user.UserName;
        //        userProfile.UserMail = user.UserMail;
        //        userProfile.Gender = user.Gender;
        //        userProfile.Dept = user.Dept;
        //        userProfile.Designation = user.Designation;
        //        userProfile.PhoneNumber = user.PhoneNumber;
        //        userProfile.Address = user.Address;

        //        _userProfileRepo.UpdateProfiles(userProfile);
        //    }

        //    try
        //    {
        //        await _userRepo.Save();
        //        await _userProfileRepo.Save();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody]UserRegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepo.RegisterUser(dto);

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
            
        }

        // DELETE: api/Users/5

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userRepo.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                await _userRepo.DeleteUser(id);
                await _userRepo.Save();
            }
            catch (Exception)
            {
                return NotFound("Error deleting the user");
            }

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


        [HttpPut("{userId}/upload")]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage(int userId, IFormFile file)
        {
            var loggedUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (loggedUser != userId)
            {
                return Unauthorized("You are not authorized to update this image.");
            }


             if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var supportedFiles = new[] { "image/jpeg", "image/png" };
            if (!supportedFiles.Contains(file.ContentType))
            {
                return BadRequest("Only JPEG or PNG formats are allowed.");
            }

            var fileName = await _userRepo.UploadProfileImage(userId, file);
            if (fileName == null)
            {
                return NotFound("User profile not found.");
            }

            return Ok(new { FileName = fileName });
        }


        //[HttpPut("{userId}/upload")]
        //[Authorize]
        //public async Task<IActionResult> UploadProfileImage(int userId, IFormFile file)
        //{
        //    var loggedUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //    if (loggedUser != userId)
        //    {
        //        return Unauthorized("You are not Authorized to Update the Image");
        //    }
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("No file uploaded.");
        //    }
        //    var supportedFiles = new[] { "image/jpeg", "image/png" };
        //    if (!supportedFiles.Contains(file.ContentType))
        //    {
        //        return BadRequest("Only JPEG or PNG format are allowed");
        //    }
        //    var fileName = await _userRepo.UploadProfileImageAsync(userId, file);
        //    if (fileName == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(new { FileName = fileName });
        //}
        //[Authorize]
        //[HttpGet("{userId}/profileImage")]
        //public async Task<IActionResult> GetProfileImage(int userId)
        //{
        //    var userProfile = await _userRepo.GetUserById(userId);
        //    if (userProfile == null || userProfile.ProfileImage == null)
        //    {
        //        var defualtImagePath = _userRepo.GetDefaultImagePath();
        //        return PhysicalFile(defualtImagePath, "image/jpeg");
        //    }
        //    using (var memoryStream = new MemoryStream(userProfile.ProfileImage))
        //    {
        //        var fileExtensions = Path.GetExtension("profile-img.jpg").ToLowerInvariant();
        //        var contentType = fileExtensions switch
        //        {
        //            ".jpg" or ".jpeg" => "image/jpeg",
        //            ".png" => "image/png",
        //            _ => "application/octet-stream"
        //        };
        //        return File(memoryStream.ToArray(), contentType);
        //    }
        //}
        [Authorize]
        [HttpGet("{userId}/profileImage")]
        public async Task<IActionResult> GetProfileImage(int userId)
        {
            var userProfile = await _userRepo.GetUserById(userId);
            if (userProfile == null || userProfile.ProfileImage == null)
            {
                var defaultImagePath = _userRepo.GetImagePath("profile-img.jpg");
                return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), defaultImagePath), "image/jpeg");
            }
            string fileName = Encoding.UTF8.GetString(userProfile.ProfileImage);
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), _userRepo.GetImagePath(fileName));

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image file not found.");
            }

            string contentType = Path.GetExtension(fileName).ToLowerInvariant() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };

            return PhysicalFile(imagePath, contentType);



}
    }
}

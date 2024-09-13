using Hexa_Hub.DTO;
using Hexa_Hub.Repository;

namespace Hexa_Hub.Interface
{
    public interface IUserRepo
    {
        Task<List<User>> GetAllUser();
        Task<User?> GetUserById(int id);
        Task<User?> GetUserId(int id);
        Task<User> UpdateUser(User user);
        Task DeleteUser(int id);
        Task Save();
        Task<User?> validateUser(string email, string password);
        Task<User> RegisterUser(UserRegisterDto dto);

        Task<string?> UploadProfileImage(int userId, IFormFile file);
        //string GetDefaultImagePath();

        public string GetImagePath(string fileName);
    }
}

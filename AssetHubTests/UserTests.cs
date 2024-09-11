using Hexa_Hub.Interface;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetHubTests
{
    internal class UserTests
    {
        private IUserRepo _userRepo;
        private Mock<IUserRepo> _userRepoMock;

        [SetUp]
        public void SetUp()
        {
            _userRepoMock = new Mock<IUserRepo>();
            _userRepo = _userRepoMock.Object;
        }

        [TestCase]
        public async Task ReturnsAllUsers()
        {
            var expectedUSersList = new List<User>()
            {
                new User{UserId = 1,UserName = "Test1"},
                new User{UserId = 2,UserName = "Test2" }
            };

            _userRepoMock.Setup(u => u.GetAllUser())
                .ReturnsAsync(expectedUSersList);

            var result = await _userRepo.GetAllUser();

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(2, result.Count, "There Should be 2 Results");
            Assert.AreEqual("Test1", result[0].UserName);
            Assert.AreEqual("Test2", result[1].UserName);
        }

        [TestCase]
        public async Task AddUser()
        {
            var user = new User { UserId = 1, UserName = "Test1" };

            _userRepoMock.Setup(ml => ml.AddUser(user))
                .Callback((User user) => { });

            await _userRepo.AddUser(user);

            _userRepoMock.Verify(u => u.AddUser(It.Is<User>(m => m.UserId == user.UserId && m.UserName == user.UserName)), Times.Once);
        }

        [TestCase]
        public async Task DeleteUser()
        {
            var id = 1;
            _userRepoMock.Setup(u => u.DeleteUser(id))
                .Callback<int>(id => { });

            await _userRepo.DeleteUser(id);

            _userRepoMock.Verify(u => u.DeleteUser(It.Is<int>(m => m == id)), Times.Once);
        }

        [TestCase]
        public async Task GetUserById()
        {
            var id = 1;
            var user = new User { UserId = id, UserName = "Test1" };

            _userRepoMock.Setup(u => u.GetUserById(id))
                .ReturnsAsync(user);

            var result = await _userRepo.GetUserById(id);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(user.UserId, result.UserId, "Should match");
            Assert.AreEqual(user.UserName, result.UserName, "Should Match");
        }

        [TestCase]
        public async Task UpdateUser()
        {
            var user = new User { UserId = 3, UserName = "Test1" };

            _userRepoMock.Setup(u => u.UpdateUser(user))
                .ReturnsAsync(user);

            var result = await _userRepo.UpdateUser(user);

            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(user.UserId, result.UserId, " Id Should Match");
            Assert.AreEqual(user.UserId, result.UserId, " Name Should Match");
        }

        [TestCase]
        public async Task ValidateUser()
        {
            var email = "test@emaple.com";
            var pass = "Test@123";
            var user = new User { UserMail = email, Password = pass };

            _userRepoMock.Setup(u => u.validateUser(email, pass)).ReturnsAsync(user);

            var result = await _userRepo.validateUser(email, pass);
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(user.UserMail, result.UserMail, " Should Match");
            Assert.AreEqual(user.Password, result.Password, " Should Match");
        }
    }
}

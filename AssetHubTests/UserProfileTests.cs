//using Hexa_Hub.Interface;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AssetHubTests
//{
//    internal class UserProfileTests
//    {
//        private IUserProfileRepo _userRepo;
//        private Mock<IUserProfileRepo> _userRepoMock;

//        [SetUp]
//        public void SetUp()
//        {
//            _userRepoMock = new Mock<IUserProfileRepo>();
//            _userRepo = _userRepoMock.Object;
//        }

//        [TestCase]
//        public async Task AddUser()
//        {
//            var user = new UserProfile { UserId = 1, UserName = "Test1" };

//            _userRepoMock.Setup(ml => ml.AddProfiles(user))
//                .Callback((UserProfile user) => { });

//            await _userRepo.AddProfiles(user);

//            _userRepoMock.Verify(u => u.AddProfiles(It.Is<UserProfile>(m => m.UserId == user.UserId && m.UserName == user.UserName)), Times.Once);
//        }

//        [TestCase]
//        public async Task DeleteUser()
//        {
//            var id = 1;
//            _userRepoMock.Setup(u => u.DeleteProfiles(id))
//                .Callback<int>(id => { });

//            await _userRepo.DeleteProfiles(id);

//            _userRepoMock.Verify(u => u.DeleteProfiles(It.Is<int>(m => m == id)), Times.Once);
//        }

//        [TestCase]
//        public async Task GetUserById()
//        {
//            var id = 1;
//            var user = new UserProfile { UserId = id, UserName = "Test1" };

//            _userRepoMock.Setup(u => u.GetProfilesById(id))
//                .ReturnsAsync(user);

//            var result = await _userRepo.GetProfilesById(id);

//            Assert.IsNotNull(result, "Result should not be null");
//            Assert.AreEqual(user.UserId, result.UserId, "Should match");
//            Assert.AreEqual(user.UserName, result.UserName, "Should Match");
//        }

//        [TestCase]
//        public async Task UpdateUser()
//        {
//            var user = new UserProfile { UserId = 3, UserName = "Test1" };

//            _userRepoMock.Setup(u => u.UpdateProfiles(user))
//                .ReturnsAsync(user);

//            var result = await _userRepo.UpdateProfiles(user);

//            Assert.IsNotNull(result, "Result should not be null");
//            Assert.AreEqual(user.UserId, result.UserId, " Id Should Match");
//            Assert.AreEqual(user.UserId, result.UserId, " Name Should Match");
//        }


//    }
//}

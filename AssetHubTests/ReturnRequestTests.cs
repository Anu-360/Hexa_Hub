//using Moq;
//using Hexa_Hub.Interface;
//using static Hexa_Hub.Models.MultiValues;

//namespace AssetHubTests
//{
//    public class ReturnRequestTests
//    {
//        private IReturnReqRepo returnrepo;
//        private Mock<IReturnReqRepo> returnrepoMock;    

//        [SetUp]

//        public void SetUp()
//        {
//            returnrepoMock = new Mock<IReturnReqRepo>();
//            returnrepo =returnrepoMock.Object;
//        }

//        [TestCase]
//        public async Task ReturnsAllReturnRequestsAsync()
//        {

//            // Arrange
//            var expectedReturnRequest = new List<ReturnRequest>
//                {
//                new ReturnRequest { ReturnId=1,ReturnStatus =ReturnReqStatus.Approved },

//                new ReturnRequest { ReturnId=2,ReturnStatus = ReturnReqStatus.Returned}
//                };

//            // Mock the repository methods
//            returnrepoMock.Setup(a => a.GetAllReturnRequest())
//                .ReturnsAsync(expectedReturnRequest);

//            // Act
//            var result = await returnrepo.GetAllReturnRequest();

//            // Assert
//            Assert.IsNotNull(result, "Result should not be null");

//            Assert.AreEqual(ReturnReqStatus.Approved, result[0].ReturnStatus);
//            Assert.AreEqual(ReturnReqStatus.Returned, result[1].ReturnStatus);
//        }

//        [TestCase]
//        public async Task AddReturnRequest_ShouldAddReturnRequest()
//        {
//            // Arrange
//            var newReturnRequest = new ReturnRequest { ReturnId = 3, ReturnStatus = ReturnReqStatus.Sent };


//            // Mock
//            returnrepoMock.Setup(repo => repo.AddReturnRequest(It.IsAny<ReturnRequest>())).Callback((ReturnRequest returnreq) => { });

//            // Act
//            await returnrepo.AddReturnRequest(newReturnRequest);

//            //Assert
//            returnrepoMock.Verify(repo => repo.AddReturnRequest(It.Is<ReturnRequest>(a => a.ReturnId == newReturnRequest.ReturnId && a.ReturnStatus == newReturnRequest.ReturnStatus)), Times.Once);
//        }

//        [TestCase]

//        public async Task Save_ShouldCallSaveChanges()
//        {
//            // Act
//            await returnrepo.Save();

//            // Assert
//            returnrepoMock.Verify(repo => repo.Save(), Times.Once);
//        }

//        [TestCase]
//        public async Task DeleteReturnRequest_ShouldRemoveReturnRequest()
//        {
//            // Arrange
//            var returnIdToDelete = 1;

//            // Mock
//            returnrepoMock.Setup(repo => repo.DeleteReturnRequest(It.IsAny<int>())).Callback<int>(id => { });


//            // Act
//            await returnrepo.DeleteReturnRequest(returnIdToDelete);

//            // Assert
//            returnrepoMock.Verify(repo => repo.DeleteReturnRequest(It.Is<int>(id => id == returnIdToDelete)), Times.Once);
//        }

//        [TestCase]
//        public void  UpdateReturnRequest_ShouldUpdateReturnRequest()
//        {
//            // Arrange
//            var updatedReturnRequest = new ReturnRequest { ReturnId = 1, ReturnStatus = ReturnReqStatus.Rejected};

//            // Mock
//            returnrepoMock.Setup(repo => repo.UpdateReturnRequest(It.IsAny<ReturnRequest>()));

//            // Act
//            returnrepoMock.Object.UpdateReturnRequest(updatedReturnRequest);

//            // Assert
//            returnrepoMock.Verify(repo => repo.UpdateReturnRequest(It.Is<ReturnRequest>(a =>
//                a.ReturnId == updatedReturnRequest.ReturnId && a.ReturnStatus == updatedReturnRequest.ReturnStatus)), Times.Once);
//        }
//    }
//}

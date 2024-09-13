//using Moq;
//using Hexa_Hub.Interface;
//using static Hexa_Hub.Models.MultiValues;

//namespace AssetHubTests
//{
//    public class AssetRequestTests
//    {
//        private IAssetRequest assetRequest;
//        private Mock<IAssetRequest> assetRequestMock;

//        [SetUp]

//        public void SetUp()
//        {
//            assetRequestMock = new Mock<IAssetRequest>();
//            assetRequest = assetRequestMock.Object;
//        }

//        [TestCase]
//        public async Task ReturnsAllAssetRequestsAsync()
//        {

//            // Arrange
//            var expectedAssetRequest = new List<AssetRequest>
//                {
//                new AssetRequest { AssetReqId=1,Request_Status=RequestStatus.Pending},
//                new AssetRequest { AssetReqId = 2, Request_Status=RequestStatus.Allocated}
//                };

//            // Mock the repository methods
//            assetRequestMock.Setup(a => a.GetAllAssetRequests())
//                .ReturnsAsync(expectedAssetRequest);

//            // Act
//            var result = await assetRequest.GetAllAssetRequests();

//            // Assert
//            Assert.IsNotNull(result, "Result should not be null");

//            Assert.AreEqual(RequestStatus.Pending, result[0].Request_Status);
//            Assert.AreEqual(RequestStatus.Allocated, result[1].Request_Status);
//        }

//        [TestCase]
//        public async Task AddAssetRequest_ShouldAddAssetRequest()
//        {
//            // Arrange
//            var newAssetRequest = new AssetRequest { AssetReqId = 3, Request_Status=RequestStatus.Rejected };


//            // Mock
//            assetRequestMock.Setup(repo => repo.AddAssetRequest(It.IsAny<AssetRequest>())).Callback((AssetRequest assetreq) => { });

//            // Act
//            await assetRequest.AddAssetRequest(newAssetRequest);

//            //Assert
//            assetRequestMock.Verify(repo => repo.AddAssetRequest(It.Is<AssetRequest>(a => a.AssetReqId == newAssetRequest.AssetReqId && a.Request_Status == newAssetRequest.Request_Status)), Times.Once);
//        }

//        [TestCase]

//        public async Task Save_ShouldCallSaveChanges()
//        {
//            // Act
//            await assetRequest.Save();

//            // Assert
//            assetRequestMock.Verify(repo => repo.Save(), Times.Once);
//        }

//        [TestCase]
//        public async Task DeleteRequest_ShouldRemoveRequest()
//        {
//            // Arrange
//            var ReqIdToDelete = 1;

//            // Mock
//            assetRequestMock.Setup(repo => repo.DeleteAssetRequest(It.IsAny<int>())).Callback<int>(id => { });


//            // Act
//            await assetRequest.DeleteAssetRequest(ReqIdToDelete);

//            // Assert
//            assetRequestMock.Verify(repo => repo.DeleteAssetRequest(It.Is<int>(id => id ==ReqIdToDelete)), Times.Once);
//        }

//        [TestCase]
//        public async Task UpdateRequest_ShouldUpdateRequest()
//        {
//            // Arrange
//            var updatedRequest = new AssetRequest { AssetReqId = 1, Request_Status= RequestStatus.Pending };

//            // Mock
//            assetRequestMock.Setup(repo => repo.UpdateAssetRequest(It.IsAny<AssetRequest>())).ReturnsAsync((AssetRequest set) => set);

//            // Act
//            var result = await assetRequest.UpdateAssetRequest(updatedRequest);

//            // Assert
//            Assert.IsNotNull(result, "Result should not be null");
//            Assert.AreEqual(updatedRequest.AssetReqId, result.AssetReqId, "AssetReq ID should match");
//            Assert.AreEqual(updatedRequest.Request_Status, result.Request_Status, "Request Status should be updated");

//            assetRequestMock.Verify(repo => repo.UpdateAssetRequest(It.Is<AssetRequest>(a =>
//                a.AssetReqId == updatedRequest.AssetReqId && a.Request_Status == updatedRequest.Request_Status)), Times.Once);
//        }
//    }
//}

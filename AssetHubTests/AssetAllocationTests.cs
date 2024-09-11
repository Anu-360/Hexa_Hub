﻿using Moq;
using Hexa_Hub.Interface;
using NuGet.ContentModel;

namespace AssetHubTests
{
    public class AssetAllocationTests
    {
        private IAssetAllocation assetAllocation;
        private Mock<IAssetAllocation> assetAllocationMock;

        [SetUp]

        public void SetUp()
        {
            assetAllocationMock = new Mock<IAssetAllocation>();
            assetAllocation = assetAllocationMock.Object;
        }

        [TestCase]
        public async Task ReturnsAllAssetAllocationsAsync()
        {

            // Arrange
            var expectedAssetAllocation = new List<AssetAllocation>
                {
                new AssetAllocation { AllocationId = 1, AssetId = 1},
                new AssetAllocation { AllocationId = 2, AssetId = 2}
                };

            // Mock the repository methods
            assetAllocationMock.Setup(a => a.GetAllAllocations())
                .ReturnsAsync(expectedAssetAllocation);

            // Act
            var result = await assetAllocation.GetAllAllocations();

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
          
            Assert.AreEqual(1, result[0].AllocationId);
            Assert.AreEqual(2, result[1].AllocationId);
        }

        [TestCase]
        public async Task AddAssetAllocation_ShouldAddAssetAllocation()
        {
            // Arrange
            var newAssetAllocation = new AssetAllocation { AllocationId = 3,AssetId=4 };


            // Mock
            assetAllocationMock.Setup(repo => repo.AddAllocation(It.IsAny<AssetAllocation>())).Callback((AssetAllocation assetalloc) => { });

            // Act
            await assetAllocation.AddAllocation(newAssetAllocation);

            //Assert
            assetAllocationMock.Verify(repo => repo.AddAllocation(It.Is<AssetAllocation>(a => a.AllocationId == newAssetAllocation.AllocationId && a.AssetId == newAssetAllocation.AssetId)), Times.Once);
        }

        [TestCase]

        public async Task Save_ShouldCallSaveChanges()
        {
            // Act
            await assetAllocation.Save();

            // Assert
            assetAllocationMock.Verify(repo => repo.Save(), Times.Once);
        }

        [TestCase]
        public async Task DeleteAllocation_ShouldRemoveAllocation()
        {
            // Arrange
            var allocationIdToDelete = 1;

            // Mock
            assetAllocationMock.Setup(repo => repo.DeleteAllocation(It.IsAny<int>())).Callback<int>(id => { });


            // Act
            await assetAllocation.DeleteAllocation(allocationIdToDelete);

            // Assert
            assetAllocationMock.Verify(repo => repo.DeleteAllocation(It.Is<int>(id => id == allocationIdToDelete)), Times.Once);
        }

        [TestCase]
        public async Task UpdateAllocation_ShouldUpdateAllocation()
        {
            // Arrange
            var updatedAllocation = new AssetAllocation { AllocationId = 1, AssetId =5 };

            // Mock
            assetAllocationMock.Setup(repo => repo.UpdateAllocation(It.IsAny<AssetAllocation>())).ReturnsAsync((AssetAllocation set) => set);

            // Act
            var result = await assetAllocation.UpdateAllocation(updatedAllocation);

            // Assert
            Assert.IsNotNull(result, "Result should not be null");
            Assert.AreEqual(updatedAllocation.AssetId, result.AssetId, "Asset ID should match");
            Assert.AreEqual(updatedAllocation.AllocationId, result.AllocationId, "Allocation ID should be updated");

            assetAllocationMock.Verify(repo => repo.UpdateAllocation(It.Is<AssetAllocation>(a =>
                a.AllocationId == updatedAllocation.AllocationId && a.AssetId == updatedAllocation.AssetId)), Times.Once);
        }


    }
}

//using Moq;
//using Hexa_Hub.Interface;
//using static Hexa_Hub.Models.MultiValues;


//namespace AssetHubTests
//{
//    public class AuditTests
//    {
//        private IAuditRepo audit;
//        private Mock<IAuditRepo> auditMock;


//        [SetUp]
//        public void SetUp()
//        {
//            auditMock = new Mock<IAuditRepo>();
//            audit = auditMock.Object;
//        }

//        [TestCase]
//        public async Task ReturnsAllAuditsAsync()
//        {

//            // Arrange
//            var expectedAudits = new List<Audit>
//                {
//                new Audit { AuditId = 1, Audit_Status=AuditStatus.Completed},
//                new Audit { AuditId = 2, Audit_Status=AuditStatus.Sent}
//                };

//            // Mock the repository methods
//            auditMock.Setup(a => a.GetAllAudits())
//                .ReturnsAsync(expectedAudits);

//            // Act
//            var result = await audit.GetAllAudits();

//            // Assert
//            Assert.IsNotNull(result, "Result should not be null");
//            Assert.AreEqual(2, result.Count, "Audit count should be 2");
//            Assert.AreEqual(AuditStatus.Completed, result[0].Audit_Status);
//            Assert.AreEqual(AuditStatus.Sent, result[1].Audit_Status);
//        }

//        [TestCase]
//        public async Task AddAudit_ShouldAddAudit()
//        {
//            // Arrange
//            var newAudit = new Audit { AuditId = 3, Audit_Status=AuditStatus.Sent};


//            // Mock
//            auditMock.Setup(repo => repo.AddAuditReq(It.IsAny<Audit>())).Callback((Audit audit) => { });

//            // Act
//            await audit.AddAuditReq(newAudit);

//            //Assert
//            auditMock.Verify(repo => repo.AddAuditReq(It.Is<Audit>(c => c.AuditId == newAudit.AuditId && c.Audit_Status == newAudit.Audit_Status)), Times.Once);
//        }

//        [TestCase]

//        public async Task Save_ShouldCallSaveChanges()
//        {
//            // Act
//            await audit.Save();

//            // Assert
//            auditMock.Verify(repo => repo.Save(), Times.Once);
//        }

//        [TestCase]
//        public async Task DeleteAudit_ShouldRemoveAudit()
//        {
//            // Arrange
//            var auditIdToDelete = 1;

//            // Mock
//            auditMock.Setup(repo => repo.DeleteAuditReq(It.IsAny<int>())).Callback<int>(id => { });


//            // Act
//            await audit.DeleteAuditReq(auditIdToDelete);

//            // Assert
//            auditMock.Verify(repo => repo.DeleteAuditReq(It.Is<int>(id => id == auditIdToDelete)), Times.Once);
//        }

//        [TestCase]
//        public async Task UpdateAudit_ShouldUpdateAudit()
//        {
//            // Arrange
//            var updatedAudit = new Audit { AuditId = 1,Audit_Status=AuditStatus.Sent };

//            // Mock
//            auditMock.Setup(repo => repo.UpdateAudit(It.IsAny<Audit>())).ReturnsAsync((Audit set) => set);

//            // Act
//            var result = await audit.UpdateAudit(updatedAudit);

//            // Assert
//            Assert.IsNotNull(result, "Result should not be null");
//            Assert.AreEqual(updatedAudit.AuditId, result.AuditId, "Audit ID should match");
//            Assert.AreEqual(updatedAudit.Audit_Status, result.Audit_Status, "Audit Status should be updated");

//            auditMock.Verify(repo => repo.UpdateAudit(It.Is<Audit>(a =>
//                a.AuditId == updatedAudit.AuditId && a.Audit_Status == updatedAudit.Audit_Status)), Times.Once);
//        }






//    }
//}

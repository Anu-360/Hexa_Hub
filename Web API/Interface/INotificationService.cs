using static Hexa_Hub.Models.MultiValues;

namespace Hexa_Hub.Interface
{
    public interface INotificationService
    {
        Task SendAllocationApproved(string UserMail, string UserName, string AssetName, int assetId);
        Task SendAllocationRejected(string UserMail, string UserName, string AssetName, int assetId);
        Task SendAudit(string UserMail, string UserName, int AuditId);
        Task AduitCompleted(string UserMail, string UserName, int AuditId);
        Task ServiceRequestSent(string UserMail, string UserName, int AssetId, int serviceId, IssueType issueType);
        Task ServiceRequestApproved(string UserMail, string UserName, int AssetId, int serviceId, IssueType issueType);
        Task ServiceRequestCompleted(string UserMail, string UserName, int AssetId, int serviceId, IssueType issueType);
        Task ReturnRequestSent(string UserMail, string UserName, int AssetId, int returnId);
        Task ReturnRequestApproved(string UserMail, string UserName, int AssetId, int returnId);
        Task ReturnRequestRejected(string UserMail, string UserName, int AssetId, int returnId);
        Task ReturnRequestCompleted(string UserMail, string UserName, int AssetId);

        Task AssetRequestSent(string UserMail, string UserName, int assetId);



    }
}

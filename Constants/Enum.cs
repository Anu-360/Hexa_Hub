using System;

public class Enum
{
    public enum AssetStatus
    {
        OpenToRequest,
        Allocated,
        UnderMaintenance
    }
    public enum UserType
    {
        Employee,
        Admin
    }

    public enum RequestStatus
    {
        Pending,
        Allocated,
        Rejected
    }

    public enum IssueType
    {
        Malfunction,
        Repair,
        Installation
    }

    public enum AuditStatus
    {
        Sent,
        Pending,
        Completed
    }

    public enum ServiceReqStatus
    {
        UnderReview,
        Approved,
        Completed
    }

    public enum ReturnReqStatus
    {
        Sent,
        Approved,
        Rejected,
        Returned
    }

}

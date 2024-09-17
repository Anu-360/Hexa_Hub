using System;

namespace Hexa_Hub.Models;
public class MultiValues
{
    public enum AssetStatus
    {
        OpenToRequest=0,
        Allocated=1,
        UnderMaintenance=2
    }

    public enum UserType
    {
        Employee=0,
        Admin=1
    }

    public enum RequestStatus
    {
        Pending=0,
        Allocated=1,
        Rejected=2
    }

    public enum IssueType
    {
        Malfunction=0,
        Repair=1,
        Installation=2
    }

    public enum AuditStatus
    {
        Sent=0,
        Completed=1
    }

    public enum ServiceReqStatus
    {
        UnderReview=0,
        Approved=1,
        Completed=2
    }

    public enum ReturnReqStatus
    {
        Sent=0,
        Approved=1,
        Returned=2,
        Rejected=3
    }

}

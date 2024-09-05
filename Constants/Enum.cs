using System;

public class Enum
{
    public enum AssetStatus
    {
        OpenToRequest,
        Allocated,
        Maintenance
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
}

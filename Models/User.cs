using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hexa_Hub.Constants;

public class User
{
    [Required]
    [Key]
    public int EmpId { get; set; }

    [Required]
    [MaxLength(55)]
    public string EmpName { get; set; }

    [Required]
    [EmailAddress]
    public string EmpMail { get; set; }

    [Required]
    public string Gender { get; set; }

    [Required]
    public string Dept { get; set; }

    [Required]
    public string Designation { get; set; }

    [Required]
    [Phone(ErrorMessage = "Please enter a valid phone number")]
    public string PhoneNumber { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string Branch { get; set; }

    [Required]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[A-Z])(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must contain Uppercase, alphanumeric and special characters")]

    public string Password { get; set; }

    [Required]
    [DefaultValue(UserType.Employee)]
    public UserType User_Type { get; private set; } = UserType.Employee;

    //Navigation Properties
    // 1 - 1 Relation

    public AuditRequest? AuditRequest { get; set; }

    public UserProfile? UserProfile { get; set; }

    // 1 - * Relation

    public ICollection<Asset>? Assets { get; set; } = new List<Asset>();

    public ICollection<AssetRequest>? AssetRequests { get; set; } = new List<AssetRequest>();

    public ICollection<ServiceRequest>? ServiceRequests { get; set; } = new List<ServiceRequest>();

    public ICollection<AssetAllocation>? AssetAllocations { get; set; } = new List<AssetAllocation>();

    public ICollection<ReturnRequest>? ReturnRequests { get; set; } = new List<ReturnRequest>();

   
}

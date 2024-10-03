using System;
using System.ComponentModel.DataAnnotations;
using Hexa_Hub.Constants;

public class UserProfile
{
    [NotMapped]
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

    public byte[]? ProfileImage { get; set; }
}

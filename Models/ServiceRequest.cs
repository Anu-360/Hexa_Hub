using System;
using System.ComponentModel.DataAnnotations;
using Hexa_Hub.Constants;

public class ServiceRequest
{
    [Required]
    [Key]
    public int ServiceId { get; set; }

    [Required]
    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    [Required]
    [ForeignKey("Employee")]
    public int EmpId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime ServiceRequestDate { get; set; }

    [Required]
    public IssueType Issue_Type { get; set; }

    [Required]
    public string ServiceDescription { get; set; }
}

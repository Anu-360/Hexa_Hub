using System;
using System.ComponentModel.DataAnnotations;
using Hexa_Hub.Constants;

public class ReturnRequest
{
    [Required]
    [Key]
    public int ReturnId { get; set; }

    [Required]
    [ForeignKey("Employee")]
    public int EmpId { get; set; }

    [Required]
    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    [Required]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime ReturnDate { get; set; }

    [Required]
    public string Reason { get; set; }

    [Required]
    public string Condition { get; set; }
}

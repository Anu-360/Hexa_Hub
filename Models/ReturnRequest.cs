using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    //Navigation Properties
    // * - 1 Relation

    public Asset? Asset { get; set; }

    //* - * Relation

    public ICollection<User>? Users { get; set; } = new List<User>(); 
}

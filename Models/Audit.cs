using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hexa_Hub.Constants;

public class Audit
{
    [Required]
    [Key]
    public int AuditId { get; set; }

    [Required]
    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    [Required]
    [ForeignKey("Employee")]
    public int EmpId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime AuditDate { get; set; }

    [Required]
    public string AuditMessage { get; set; }

    //Navigation Properties
    // 1 - 1 Relation

    public Asset? Asset { get; set; }
}

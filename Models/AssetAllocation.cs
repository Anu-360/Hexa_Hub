using System;
using System.ComponentModel.DataAnnotations;
using Hexa_Hub.Constants;

public class AssetAlocation
{
    [Required]
    [Key]
    public int AllocationId { get; set; }

    [Required]
    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    [Required]
    [ForeignKey("User")]
    public int EmpId { get; set; }

    [Required]
    [ForeignKey("AssetRequest")]
    public int AssetReqId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime AllocatedDate { get; set; }
}

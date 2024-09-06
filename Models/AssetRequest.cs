using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hexa_Hub.Constants;


public class AssetRequest
{
    [Required]
    [Key]
    public int AssetReqId { get; set; }

    [Required]
    [ForeignKey("Employee")]
    public int EmpId { get; set; }

    [Required]
    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    [Required]
    [ForeignKey("Category")]
    public string CategoryId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime AssetReqDate { get; set; }

    [Required]
    public string AssetReqReason { get; set; }

    [Required]
    [DefaultValue(RequestStatus.Pending)]
    public RequestStatus Request_Status { get; set; } = RequestStatus.Pending;

    //Navigation Properties
    // 1 - 1 Relation

    public Asset? Asset {  get; set; }
}

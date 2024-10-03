using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hexa_Hub;

public class Asset
{
    [Required]
    [Key]
    public int AssetId { get; set; }

    [Required]
    [MaxLength(55)]
    public string AssetName { get; set; }

    public string? AssetDescription { get; set; }

    [Required]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    [Required]
    [ForeignKey("SubCategory")]
    public int SubCategoryId { get; set; }

    public byte[]? AssetImage { get; set; }

    [Required]
    public string SerialNumber { get; set; }

    [Required]
    public string Model { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime ManufacturingDate { get; set; }

    [Required]
    [MaxLength(55)]
    public string Location { get; set; }

    [Required]
    public decimal Value { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime? Expiry_Date { get; set; }

    [Required]
    [DefaultValue(AssetStatus.OpenToRequest)]
    public AssetStatus Asset_Status { get; set; } = AssetStatus.OpenToRequest;
}

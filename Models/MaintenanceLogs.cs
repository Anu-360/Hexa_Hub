using System;
using System.ComponentModel.DataAnnotations;
using Hexa_Hub.Constants;

public class Maintenancelogs
{
    [Required]
    [Key]
    public int MaintenanceId { get; set; }

    [Required]
    [ForeignKey("Asset")]
    public int AssetId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime Maintenance_date { get; set; }

    public decimal? Cost { get; set; }

    public string? Maintenance_Description { get; set; }
}

using System;
using System.ComponentModel.DataAnnotations;
using Hexa_Hub.Constants;

public class SubCategory
{
    [Required]
    [Key]
    public int SubCategoryId { get; set; }

    [Required]
    [MaxLength(55)]
    public string SubCategoryName { get; set; }

    [Required]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    [Required]
    public int Quantity { get; set; }
}

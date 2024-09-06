using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    //Navigation Properties
    // 1 - 1 Relation

    public Category? Category { get; set; }  
}

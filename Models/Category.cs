using System;
using System.ComponentModel.DataAnnotations;
using Hexa_Hub.Constants;

public class Category
{
    [Required]
    [Key]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(55)]
    public string CategoryName { get; set; }
}

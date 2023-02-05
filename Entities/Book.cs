using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WEB_API.Entities;

[Table("Book")]
public sealed class Book
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int ID { get; set; }

    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }

    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public double Price { get; set; }

    [Range(0, 100)]
    public int Quantity { get; set; }
}
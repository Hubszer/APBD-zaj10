using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Models;

[Table("editions")]
public class Edition
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public int PublicationYear { get; set; }

    public int  BookId { get; set; }
    
    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; } = null!;
}
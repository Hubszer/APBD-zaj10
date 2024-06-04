using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models;

[Table("books")]
public class Book
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;
    [Precision(3)]
    public double Price { get; set; }
    public int TotalPages { get; set; }

    public ICollection<Author> Authors { get; set; } = new HashSet<Author>();
    
    public ICollection<Edition> Editions { get; set; } = new HashSet<Edition>();
    
    public ICollection<BookAward> BookAwards { get; set; } = new HashSet<BookAward>();
}
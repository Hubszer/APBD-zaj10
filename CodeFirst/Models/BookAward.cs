using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models;

/*[Table("book_awards")]
[PrimaryKey(nameof(BookId), nameof(AwardId))]*/
[Table("book_awards")]
[PrimaryKey(nameof(BookId),nameof(AwardId))]
public class BookAward
{
    /*public int Year { get; set; }
    public int BookId { get; set; }
    public int AwardId { get; set; }
    [ForeignKey(nameof(BookId))]
    
    public Book Book { get; set; } = null!;
    [ForeignKey(nameof(AwardId))]
    public Award Award { get; set; } = null!;*/
    [Key]
    public int  BookId { get; set; }
    
    public int  AwardId { get; set; }
    
    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; }
    
    [ForeignKey(nameof(AwardId))]
    public Award Award { get; set; }
}
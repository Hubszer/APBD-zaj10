using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace Zad10.Models;

[Table("patient")]
public class Patient
{
    [Key] 
    public int IdPatient { get; set; }
    [MaxLength(100)]
    public string FirstName { get; set; }
    [MaxLength(100)] 
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; } = new HashSet<Prescription>();
}
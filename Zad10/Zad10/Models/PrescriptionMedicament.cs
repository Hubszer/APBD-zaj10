using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zad10.Models;

[Table("prescriptionMedicament")]
public class PrescriptionMedicament
{
    [Key]
    public int IdMedicament { get; set; }
    
    [ForeignKey(nameof(IdMedicament))]
    public Medicament Medicament { get; set; }

    [Key] 
    public int IdPrescription { get; set; }
    
    [ForeignKey(nameof(IdPrescription))] 
    public Prescription Prescription { get; set; }
    
    public int  Dose { get; set; }
    
    [MaxLength(100)] 
    public string Details { get; set; }
    
    
}
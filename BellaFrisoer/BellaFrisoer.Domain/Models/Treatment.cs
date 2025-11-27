using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellaFrisoer.Domain.Models;

public class HairTreatment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty; // Navn på behandlingen

    [Required]
    [StringLength(50)]
    public string ProductNumber { get; set; } = string.Empty; // Produktnummer (fx "PRD-203")

    [Range(1, 480)]
    public int DurationMinutes { get; set; } // Tidsforbrug i minutter

    [Range(0, 5000)]
    public decimal HourlyRate { get; set; } // Timepris i kr.

    
    // public decimal TotalPrice => (DurationMinutes / 60m) * HourlyRate;
}

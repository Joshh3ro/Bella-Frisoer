using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellaFrisoer.Domain.Models;

public class Treatment
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty; // Navn på behandlingen

    [Required]
    public decimal Price { get; set; } // Pris i kr.
    [Required]
    public int Duration { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}

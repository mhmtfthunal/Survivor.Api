using System.ComponentModel.DataAnnotations;

namespace Survivor.Api.Entities;

public class Competitor : BaseEntity
{
    [Required, MaxLength(50)]
    public string FirstName { get; set; } = default!;
    [Required, MaxLength(50)]
    public string LastName { get; set; } = default!;

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}

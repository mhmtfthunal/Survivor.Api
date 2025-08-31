using System.ComponentModel.DataAnnotations;

namespace Survivor.Api.Entities;

public class Category : BaseEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    public ICollection<Competitor> Competitors { get; set; } = new List<Competitor>();
}

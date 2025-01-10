using System.ComponentModel.DataAnnotations;

namespace FloraFauna_GO_Entities;

public abstract class BaseEntity
{
    [Key] public string? Id { get; set; } = Guid.NewGuid().ToString();
}
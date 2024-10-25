using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Animal : AuditEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int AnimalTypeId { get; set; }

    [ForeignKey(nameof(AnimalTypeId))]
    public AnimalType AnimalType { get; set; }
}
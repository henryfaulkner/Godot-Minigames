using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Animal : AuditEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public int StomachMax { get; set; }
    public int LoveMax { get; set; }
    public int AnimalTypeId { get; set; }
    public int EggId { get; set; }

    [ForeignKey(nameof(AnimalTypeId))]
    public AnimalType AnimalType { get; set; }
    [ForeignKey(nameof(EggId))]
    public Egg Egg { get; set; }
}
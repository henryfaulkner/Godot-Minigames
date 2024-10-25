using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AnimalEvent : AuditEntity
{
    [Key]
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public int AnimalEventTypeId { get; set; }

    [ForeignKey(nameof(AnimalId))]
    public Animal Animal { get; set; }
    [ForeignKey(nameof(AnimalEventTypeId))]
    public AnimalEventType AnimalEventType { get; set; }
}
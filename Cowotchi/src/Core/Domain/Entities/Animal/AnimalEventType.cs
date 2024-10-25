using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AnimalEventType : AuditEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}
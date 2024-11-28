using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AnimalType : AuditEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Mesh { get; set; }
    public int BgEggController { get; set; }
    public int BgAnimalController { get; set; }
    public int FgEggController { get; set; }
    public int FgAnimalController { get; set; }
}
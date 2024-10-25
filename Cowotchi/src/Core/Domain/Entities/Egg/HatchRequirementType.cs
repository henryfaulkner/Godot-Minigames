using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HatchRequirementType : AuditEntity
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}
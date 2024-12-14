using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class HatchRequirement : AuditEntity
{
	[Key]
	public int Id { get; set; }
	public int HatchRequirementTypeId { get; set; }
	public int Goal { get; set; }

	[ForeignKey(nameof(HatchRequirementTypeId))]
	public HatchRequirementType HatchRequirementType { get; set; }
}

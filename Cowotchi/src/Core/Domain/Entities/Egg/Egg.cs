using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Egg : AuditEntity
{
	[Key]
	public int Id { get; set; }
	public string Name { get; set; }
	public int AnimalTypeId { get; set; }
	public bool IsHatched { get; set; }
	public bool IsInGallery { get; set; }

	[ForeignKey(nameof(AnimalTypeId))]
	public AnimalType AnimalType { get; set; }
}

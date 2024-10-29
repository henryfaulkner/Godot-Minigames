using Godot;
using System.Threading.Tasks;

public partial class EggController : CharacterBody3D, IEggController
{
	[Export]
	public AnimalController InnerAnimal { get; set; }

	public async Task Bounce() {}

	public async Task Hatch() 
	{
		InnerAnimal.Hatch();
	}    
}

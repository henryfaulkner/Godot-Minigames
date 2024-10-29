using Godot; 
using System.Threading.Tasks;

public partial class AnimalController : CharacterBody3D, IAnimalController
{
	public async Task Hatch() {}
	public async Task Eat() {}
	public async Task ReceiveLove() {}
}

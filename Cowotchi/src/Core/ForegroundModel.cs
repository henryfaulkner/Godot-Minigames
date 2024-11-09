using Godot;

public partial class ForegroundModel : CharacterBody3D, IForegroundModel
{
	public IExecuter Executer { get; set; }

	public override void _Ready() { }
}

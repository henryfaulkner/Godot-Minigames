using Godot;
using System;

public partial class LoveEffect : RigidBody3D
{
	[ExportGroup("Particle Effects")]
	[Export]
	private GpuParticles3D Hearts { get; set; }

	public override void _Ready()
	{
		Hearts.OneShot = true;
		Hearts.Emitting = false;
	}

	public void ReadyInstance(Vector3 position)
	{
		RigidBody3D.Position = position;
	}

	public void EmitEffect()
	{
		Hearts.Emitting = true;
	}
}

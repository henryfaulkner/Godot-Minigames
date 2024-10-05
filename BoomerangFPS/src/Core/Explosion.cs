// I used this tutorial: https://www.youtube.com/watch?v=RtJJVjjM_-Q

using Godot;
using System;

public partial class Explosion : RigidBody3D
{
	[ExportGroup("Particle Effects")]
	[Export]
	private GpuParticles3D Debris { get; set; }
	
	[Export]
	private GpuParticles3D Fire { get; set; }
	
	[Export]
	private GpuParticles3D Smoke { get; set; }
	
	[ExportGroup("Audio")]
	[Export]
	private AudioStreamPlayer3D ExplosionSound { get; set; }
	
	public void Explode()
	{
		Debris.Emitting = true;
		Debris.Emitting = true;
		Debris.Emitting = true;
		ExplosionSound.Play();
		TimingFunctions.SetTimeout(() => {
			QueueFree();
		}, 2000);
	}
}

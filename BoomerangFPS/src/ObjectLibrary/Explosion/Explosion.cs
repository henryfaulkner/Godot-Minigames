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
	
	[ExportGroup("Timer")]
	[Export]
	private Timer Timer { get; set; }
	
	public override void _Ready()
	{
		Timer.Timeout += Explode;
	}
	
	public void Explode()
	{
		try
		{
			Debris.Emitting = true;
			Fire.Emitting = true;
			Smoke.Emitting = true;
			ExplosionSound.Play();
			GD.Print("Explode");
			TimingFunctions.SetTimeout(() => {
				QueueFree();
			}, 2000);
		} catch (Exception ex)
		{
			GD.Print($"Explode exception: {ex.Message}");
		}
	}
}

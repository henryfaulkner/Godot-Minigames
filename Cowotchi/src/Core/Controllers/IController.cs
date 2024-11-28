using Godot;

public interface IController
{
	void SetPuppet(CharacterBody3D puppet);
	void ReadyInstance(CollisionShape3D collider, MeshInstance3D mesh);
	void PhysicsProcess(double delta);
}

using Godot;
using System;

public partial class Character : MeshInstance3D
{
	private Mesh TapMesh = (ArrayMesh)ResourceLoader.Load("res://Assets/3dModels/OBJ/TapHand.obj");
	private Mesh GrabMesh = (ArrayMesh)ResourceLoader.Load("res://Assets/3dModels/OBJ/GrabHand.obj");
	private Mesh KnockMesh = (ArrayMesh)ResourceLoader.Load("res://Assets/3dModels/OBJ/KnockHand.obj");

	public override void _Ready()
	{
		// initialize using tap mesh 
		Mesh = TapMesh;
	}

	public void Tap()
	{
		Mesh = TapMesh;
	}

	public void Grab()
	{
		Mesh = GrabMesh;
	}

	public void Knock()
	{
		Mesh = KnockMesh;
	}
}

using Godot;
using System;

public partial class Main : Node3D
{
	private ICommonInteractor _commonInteractor { get; set; } 

	public override void _Ready()
	{
		_commonInteractor = GetNode<ICommonInteractor>("/root/CommonInteractor");

		_commonInteractor.InitDatabaseIfRequired();
	}
}

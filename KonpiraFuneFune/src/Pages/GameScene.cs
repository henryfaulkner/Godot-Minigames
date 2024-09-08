using Godot;
using System;

public partial class GameScene : Node3D
{
	[Export]
	private float Speed { get; set; }

	[Export]
	private PathFollow3D Player1_PathFollow { get; set; }

	[Export]
	private Controller Player1_Controller { get; set; }

	private Player Player1 { get; set; }

	private int CommanderIdIndex { get; set; }

	private CommandService _serviceCommand;

	public override void _Ready()
	{
		_serviceCommand = GetNode<CommandService>("/root/CommandService");
		if (_serviceCommand == null)
		{
			GD.Print("GameScene: Service Command is null");
			return;
		}
		_serviceCommand.Init();

		CommanderIdIndex = 0;
		Player1 = CreatePlayer1(CommanderIdIndex);
		CommanderIdIndex = 1;
	}

	public override void _PhysicsProcess(double delta)
	{
		bool player1HitSurface = ProcessPathFollow(Player1_PathFollow, delta);
		if (player1HitSurface && Player1.LatestCommand != null) Player1.LatestCommand.Execute();
	}

	private bool ProcessPathFollow(PathFollow3D pf, double delta)
	{
		var result = false;
		if (pf == null)
		{
			GD.PrintErr("PathFollow3D is not assigned.");
			return result;
		}

		if (pf.ProgressRatio + Speed * (float)delta > 1)
		{
			pf.ProgressRatio = 1.0f;
			Speed *= -1;
		}
		else if (pf.ProgressRatio + Speed * (float)delta < 0)
		{
			pf.ProgressRatio = 0.0f;
			Speed *= -1;
			result = true;
		}
		else
		{
			pf.ProgressRatio += Speed * (float)delta;
		}
		return result;
	}

	public Player CreatePlayer1(int id)
	{
		var result = new Player(_serviceCommand);
		result.Id = id;
		result.Name = "Player 1";
		result.PathFollow = Player1_PathFollow;
		result.Controller = Player1_Controller;
		result.PlayerConfigBusiness = new PlayerConfigBusiness("Player1.json");
		result.Controller.Command += result.SetLatestCommand;
		result.SetInputKeys(result.PlayerConfigBusiness.Load());
		return result;
	}
}

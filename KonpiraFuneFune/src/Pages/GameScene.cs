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

	private PlayerConfigBusiness Player1_PlayerConfigBusiness { get; set; }

	private ICommand Player1_Command { get; set; }

	public override void _Ready()
	{
		Player1_PlayerConfigBusiness = new PlayerConfigBusiness("Player1.json");
		Player1_Controller.Command += SetPlayer1Command;
		SetPlayerInputs(Player1_Controller, Player1_PlayerConfigBusiness.Load());
	}

	public override void _PhysicsProcess(double delta)
	{
		bool player1HitSurface = ProcessPathFollow(Player1_PathFollow, delta);
		if (player1HitSurface && Player1_Command != null) Player1_Command.Execute();
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

	private void SetPlayerInputs(Controller controller, PlayerConfigModel config)
	{
		GD.Print($"config.TapInputKey {config.TapInputKey}");
		GD.Print($"config.GrabInputKey {config.GrabInputKey}");
		GD.Print($"config.KnockInputKey {config.KnockInputKey}");
		controller.SetTapInput(config.TapInputKey);
		controller.SetGrabInput(config.GrabInputKey);
		controller.SetKnockInput(config.KnockInputKey);
	}

	private void SetPlayer1Command(int commandType)
	{
		switch (commandType)
		{
			case (int)Enumerations.Commands.Tap:
				Player1_Command = CommandFactory.CreateTapCommand();
				break;
			case (int)Enumerations.Commands.Grab:
				Player1_Command = CommandFactory.CreateGrabCommand();
				break;
			case (int)Enumerations.Commands.Knock:
				Player1_Command = CommandFactory.CreateKnockCommand();
				break;
			default:
				GD.Print("SetPlayer1Command no mapping found");
				break;
		}
	}
}

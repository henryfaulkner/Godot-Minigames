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
		ProcessPathFollow(Player1_PathFollow, delta);
	}

	private void ProcessPathFollow(PathFollow3D pf, double delta)
	{
		if (pf == null)
		{
			GD.PrintErr("PathFollow3D is not assigned.");
			return;
		}

		pf.ProgressRatio += Speed * (float)delta;
	}

	private void SetPlayerInputs(Controller controller, PlayerConfigModel config)
	{
		GD.Print($"config.TapInputType {config.TapInputType}");
		GD.Print($"config.GrabInputType {config.GrabInputType}");
		GD.Print($"config.KnockInputType {config.KnockInputType}");
		controller.SetTapInput(config.TapInputType);
		controller.SetGrabInput(config.GrabInputType);
		controller.SetKnockInput(config.KnockInputType);
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

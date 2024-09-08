using Godot;

public class Player : AbstractCommander
{
	private readonly CommandService _serviceCommand;

	public Player(CommandService serviceCommand)
	{
		_serviceCommand = serviceCommand;
	}

	public override int Id { get; set; }
	public override string Name { get; set; }
	public PathFollow3D PathFollow { get; set; }
	public Controller Controller { get; set; }
	public PlayerConfigBusiness PlayerConfigBusiness { get; set; }
	public ICommand LatestCommand { get; set; }

	public void SetInputKeys(PlayerConfigModel config)
	{
		GD.Print($"config.TapInputKey {config.TapInputKey}");
		GD.Print($"config.GrabInputKey {config.GrabInputKey}");
		GD.Print($"config.KnockInputKey {config.KnockInputKey}");
		Controller.SetTapInput(config.TapInputKey);
		Controller.SetGrabInput(config.GrabInputKey);
		Controller.SetKnockInput(config.KnockInputKey);
	}

	public void SetLatestCommand(int commandType)
	{
		switch (commandType)
		{
			case (int)Enumerations.Commands.Tap:
				LatestCommand = CommandFactory.CreateTapCommand(this, _serviceCommand);
				break;
			case (int)Enumerations.Commands.Grab:
				LatestCommand = CommandFactory.CreateGrabCommand(this, _serviceCommand);
				break;
			case (int)Enumerations.Commands.Knock:
				LatestCommand = CommandFactory.CreateKnockCommand(this, _serviceCommand);
				break;
			default:
				GD.Print("SetPlayerCommand no mapping found");
				break;
		}
	}
}

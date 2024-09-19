using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class GameScene : Node3D
{
	[Export]
	private Bowl Bowl { get; set; }

	[Export]
	private float Speed { get; set; }

	[Export]
	private PathFollow3D Player1_PathFollow { get; set; }

	[Export]
	private RemoteTransform3D Player1_BowlTransform { get; set; }

	[Export]
	private Controller Player1_Controller { get; set; }

	[Export]
	private PathFollow3D Player2_PathFollow { get; set; }

	[Export]
	private RemoteTransform3D Player2_BowlTransform { get; set; }

	[Export]
	private Controller Player2_Controller { get; set; }

	private const string BOWL_RELATIVE_PATH_FROM_REMOTE_TRANSFORMS = "../../../Bowl";
	private const string PLAYER1_CONFIG_FILE_NAME = "Player1";
	private const string PLAYER2_CONFIG_FILE_NAME = "Player2";

	private Player Player1 { get; set; }
	private Player Player2 { get; set; }
	private List<Player> PlayerList { get; set; }

	private int CommanderIdIndex { get; set; }
	private List<AbstractCommander> CommanderList { get; set; }
	private CommandService _serviceCommand;

	private bool PlayerOneInitHit { get; set; }

	public override void _Ready()
	{
		PlayerOneInitHit = false;
		PlayerList = new List<Player>();
		CommanderList = new List<AbstractCommander>();

		_serviceCommand = GetNode<CommandService>("/root/CommandService");
		if (_serviceCommand == null)
		{
			GD.Print("GameScene: Service Command is null");
			return;
		}

		_serviceCommand.Init();
		_serviceCommand.AttachBowlToHand += AttachBowlToPlayer;
		_serviceCommand.DetachBowlFromHand += DetachBowlFromPlayer;

		try
		{
			Player1 = CreatePlayer1();
			PlayerList.Add(Player1);
			CommanderList.Add(Player1);
		}
		catch (Exception exception)
		{
			GD.Print($"Player1 creation failed. {exception.Message}");
		}

		try
		{
			Player2 = CreatePlayer2();
			PlayerList.Add(Player2);
			CommanderList.Add(Player2);
		}
		catch (Exception exception)
		{
			GD.Print($"Player2 creation failed. {exception.Message}");
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Player1 != null)
		{
			bool player1HitSurface = ProcessPathFollow(Player1, delta);
			if (player1HitSurface && Player1.LatestCommand != null)
			{
				PlayerOneInitHit = true;
				Player1.LatestCommand.Execute();
			}
		}

		if (Player2 != null && PlayerOneInitHit)
		{
			bool playerHitSurface = ProcessPathFollow(Player2, delta);
			if (playerHitSurface && Player2.LatestCommand != null)
			{
				Player2.LatestCommand.Execute();
			}
		}

		if (Player1 != null && Player2 != null)
		{
			// GD.Print($"Player1 PathFollow Ratio {Player1.PathFollow.ProgressRatio}");
			// GD.Print($"Player2 PathFollow Ratio {Player2.PathFollow.ProgressRatio}");
		}
	}

	private bool ProcessPathFollow(Player player, double delta)
	{
		var result = false;
		if (player.PathFollow == null)
		{
			GD.PrintErr("PathFollow3D is not assigned.");
			return result;
		}

		if (player.PathFollow.ProgressRatio + (Speed * player.DirectionSign) * (float)delta > 1)
		{
			player.PathFollow.ProgressRatio = 1.0f;
			player.DirectionSign *= -1;
		}
		else if (player.PathFollow.ProgressRatio + (Speed * player.DirectionSign) * (float)delta < 0)
		{
			player.PathFollow.ProgressRatio = 0.0f;
			player.DirectionSign *= -1;
			result = true;
		}
		else
		{
			player.PathFollow.ProgressRatio += (Speed * player.DirectionSign) * (float)delta;
		}
		return result;
	}

	private void AttachBowlToPlayer(int commanderId)
	{
		try
		{
			GD.Print("AttachBowlToPlayer + ", commanderId);
			var player = GetPlayerByCommanderId(commanderId);
			AttachBowlToPathFollow(player);
		}
		catch (Exception ex)
		{
			GD.Print($"exception: {ex.Message}");
		}
	}

	private void DetachBowlFromPlayer(int commanderId)
	{
		try
		{
			GD.Print("DetachBowlFromPlayer + ", commanderId);
			var player = GetPlayerByCommanderId(commanderId);
			DetachBowlFromPathFollow(player);
		}
		catch (Exception ex)
		{
			GD.Print($"exception: {ex.Message}");
		}
	}

	private void DetachBowlFromPlayers(List<Player> players)
	{
		players.ForEach(player => DetachBowlFromPathFollow(player));
	}

	private Player CreatePlayer1()
	{
		var result = new Player(_serviceCommand, PLAYER1_CONFIG_FILE_NAME);

		result.PathFollow = Player1_PathFollow;
		result.DirectionSign = 1;
		result.BowlTransform = Player1_BowlTransform;
		result.Controller = Player1_Controller;
		result.LatestCommand = CommandFactory.CreateTapCommand(result, _serviceCommand);

		var pcb = new PlayerConfigBusiness();
		var config = pcb.Load(result.ConfigFileName);
		result.Id = config.CommanderId;
		result.Name = config.Name;
		result.SetInputKeys(config);

		// Command func must be set after InputKeys are initialized
		result.Controller.Command += result.SetLatestCommand;

		return result;
	}

	private Player CreatePlayer2()
	{
		var result = new Player(_serviceCommand, PLAYER2_CONFIG_FILE_NAME);
		var pcb = new PlayerConfigBusiness();
		var config = pcb.Load(result.ConfigFileName);

		result.PathFollow = Player2_PathFollow;
		result.DirectionSign = 1;
		result.BowlTransform = Player2_BowlTransform;
		result.Controller = Player2_Controller;
		result.Controller.Command += result.SetLatestCommand;
		result.LatestCommand = CommandFactory.CreateTapCommand(result, _serviceCommand);

		result.Id = config.CommanderId;
		result.Name = config.Name;
		result.SetInputKeys(config);

		return result;
	}

	public Player GetPlayerByCommanderId(int commanderId)
	{
		return PlayerList.Where(x => x.Id == commanderId).First();
	}

	private void AttachBowlToPathFollow(Player player)
	{
		DetachBowlFromPlayers(PlayerList);
		player.BowlTransform.RemotePath = BOWL_RELATIVE_PATH_FROM_REMOTE_TRANSFORMS;
		player.BowlTransform.UpdatePosition = true;
	}

	private void DetachBowlFromPathFollow(Player player)
	{
		player.BowlTransform.RemotePath = null;
		player.BowlTransform.UpdatePosition = false;
	}
}

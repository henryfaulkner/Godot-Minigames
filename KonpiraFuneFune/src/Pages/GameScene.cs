using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public partial class GameScene : Node3D
{
	[Export]
	private Bowl Bowl { get; set; }

	[Export]
	private float Speed { get; set; }

	[Export]
	private PathFollow3D Player1_PathFollow { get; set; }

	[Export]
	private Controller Player1_Controller { get; set; }

	private Player Player1 { get; set; }
	private List<Player> PlayerList { get; set; }

	private int CommanderIdIndex { get; set; }
	private List<AbstractCommander> CommanderList { get; set; }
	private CommandService _serviceCommand;

	public override void _Ready()
	{
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
		_serviceCommand.DetachBowlFromHand += AttachBowlToBase;

		CommanderIdIndex = 0;
		Player1 = CreatePlayer1(CommanderIdIndex);
		PlayerList.Add(Player1);
		CommanderList.Add(Player1);
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

	private void AttachBowlToBase(int commanderId)
	{
		GD.Print($"AttachBowlToBase");
		AttachBowlTo(Bowl, this);
	}

	private void AttachBowlToPlayer(int commanderId)
	{
		try
		{
			GD.Print("AttachBowlToPlayer + ", commanderId);
			var player = GetPlayerByCommanderId(commanderId);
			AttachBowlTo(Bowl, player.PathFollow);
		}
		catch (Exception ex)
		{
			GD.Print($"exception: {ex.Message}");
		}
	}

	private void AttachBowlTo(Bowl bowl, Node3D target)
	{
		if (bowl == null)
		{
			GD.PrintErr("Bowl is null. Cannot attach it to the target.");
			return;
		}
		if (target == null)
		{
			GD.PrintErr("Target is null. Cannot attach the bowl.");
			return;
		}
		
		if (!(bowl is Node3D))
		{
			GD.PrintErr("Bowl is not a Node3D. AddChild will fail.");
			return;
		}
		
		if (bowl.GetParent() != null)
		{
			GD.Print("Bowl already has a parent. Removing from current parent.");
			bowl.GetParent().RemoveChild(bowl);
		}
		
		if (!target.IsInsideTree())
		{
			GD.PrintErr("Target is not inside the scene tree. AddChild might fail.");
			return;
		}
		
		target.AddChild(bowl);
		bowl.Owner = target;
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

	public Player GetPlayerByCommanderId(int commanderId)
	{
		return PlayerList.Where(x => x.Id == commanderId).First();
	}
}

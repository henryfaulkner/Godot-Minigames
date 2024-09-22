using Godot;
using System;
using System.ComponentModel;

public partial class Main : Node
{
	private static readonly StringName _TAB_INPUT = new StringName("tab");
	private static readonly StringName _ENTER_INPUT = new StringName("enter");

	private static readonly StringName _PLAY_SCENE = new StringName("res://src/Pages/GameScene.tscn");

	[Export]
	private Button PlayBtn { get; set; }

	[Export]
	private Button OptionsBtn { get; set; }

	[Export]
	private Button QuitBtn { get; set; }

	[Export]
	private AudioStreamPlayer SwitchAudio { get; set; }

	[Export]
	private AudioStreamPlayer SelectAudio { get; set; }

	private Button[] Buttons { get; set; }
	private int FocusIndex { get; set; }

	public override void _Ready()
	{
		SubscribeToButtonEvents();
		Buttons = new Button[] { PlayBtn, OptionsBtn, QuitBtn };
		Buttons[0].GrabFocus();
		FocusIndex = 0;
	}

	public override void _Process(double _delta)
	{
		if (Input.IsActionJustPressed(_TAB_INPUT))
		{
			SwitchAudio.Play();
			int len = Buttons.Length;
			if (FocusIndex == len - 1)
			{
				Buttons[0].GrabFocus();
				FocusIndex = 0;
			}
			else
			{
				Buttons[FocusIndex + 1].GrabFocus();
				FocusIndex += 1;
			}
		}
	}

	public void HandlePlayPressed()
	{
		SelectAudio.Play();
		HelperFunctions.SetTimeout(GetPlayAction(), 50);
	}

	public void HandleOptionsPressed()
	{
		SelectAudio.Play();
	}


	public void HandleQuitPressed()
	{
		SelectAudio.Play();
		HelperFunctions.SetTimeout(GetQuitAction(), 100);
	}

	private void SubscribeToButtonEvents()
	{
		PlayBtn.Pressed += HandlePlayPressed;
		OptionsBtn.Pressed += HandleOptionsPressed;
		QuitBtn.Pressed += HandleQuitPressed;
	}

	private Action GetPlayAction()
	{
		return () =>
		{
			try
			{
				var scene = GD.Load<PackedScene>(_PLAY_SCENE);
				GetTree().ChangeSceneToPacked(scene);
			}
			catch (Exception e)
			{
				GD.PrintErr($"Play error: {e.Message}");
			}
		};
	}

	private Action GetQuitAction()
	{
		return () =>
		{
			try
			{
				GetTree().Quit();
			}
			catch (Exception e)
			{
				GD.PrintErr($"Quit error: {e.Message}");
			}
		};
	}
}

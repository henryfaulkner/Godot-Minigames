using Godot;
using System;

public partial class Menu : CanvasLayer
{
	[ExportGroup("Pages")]
	[Export]
	private EggPage EggPage { get; set; }
	[Export]
	private AnimalPage AnimalPage { get; set; }

	[ExportGroup("Containers")]
	[Export]
	private MarginContainer MeterContainer { get; set; }
	[Export]
	private InfoContainer InfoContainer { get; set; }

	[ExportGroup("Meters")]
	[Export]
	public Meter LoveMeter { get; set; }
	[Export]
	public Meter StomachMeter { get; set; }

	private Observables _observables;

	public bool IsOn_MeterContainer { get; private set; }
	public bool IsOn_InfoContainer { get; private set; }

	public override void _Ready()
	{
		_observables = GetNode<Observables>(Constants.SingletonNodes.Observables);

		_observables.UpdateHeartMeterValue += LoveMeter.UpdateValue;
		_observables.UpdateHeartMeterMax += LoveMeter.UpdateMax;

		_observables.UpdateHungerMeterValue += StomachMeter.UpdateValue;
		_observables.UpdateHungerMeterMax += StomachMeter.UpdateMax;
	}

	public void SwapPage(Enumerations.MenuPageType type)
	{
		switch (type)
		{
			case Enumerations.MenuPageType.Egg:
				ShowEggPage();
				break;
			case Enumerations.MenuPageType.Animal:
				ShowAnimalPage();
				break;
			default:
				break;
		}
	}

	private void ShowEggPage()
	{
		EggPage.Show();
		AnimalPage.Hide();
	}

	private void ShowAnimalPage()
	{
		EggPage.Hide();
		AnimalPage.Show();
	}

	public void ToggleInfoContainer(bool on)
	{
		if (on) 
		{
			InfoContainer.Modulate = new Color(1, 1, 1, 1);
			IsOn_InfoContainer = on;
		}
		else 
		{
			InfoContainer.Modulate = new Color(1, 1, 1, 0);
			IsOn_InfoContainer = on;
		}
	}

	public void ToggleMeterContainer(bool on)
	{
		if (on) 
		{
			MeterContainer.Modulate = new Color(1, 1, 1, 1);
			IsOn_MeterContainer = on;
		}
		else 
		{
			MeterContainer.Modulate = new Color(1, 1, 1, 0);
			IsOn_MeterContainer = on;
		}
	}

	public void SetCreatureInfo(CreatureModel model)
	{
		InfoContainer.Title.Text = model.Name;
		InfoContainer.Subtitle.Text = model.CreatureType.GetDescription();
		InfoContainer.Content.Text = "";
	}
}

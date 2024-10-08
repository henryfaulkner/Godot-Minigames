using System;
using System.Collections.Generic;
using Godot;

public class PowerUpService
{
	private const int POWER_UP_LIMIT = 2;

	public Queue<Enumerations.PowerUps> CurrentPowerUps { get; private set; }
	public Stack<Enumerations.PowerUps> AvailablePowerUps { get; private set; }

	public PowerUpService()
	{
		CurrentPowerUps = new Queue<Enumerations.PowerUps>();
		AvailablePowerUps = new Stack<Enumerations.PowerUps>();
	}

	public Stack<Enumerations.PowerUps> InitPowerUpStack()
	{
		var result = new Stack<Enumerations.PowerUps>();
		var powerUpList = new List<Enumerations.PowerUps>((Enumerations.PowerUps[])Enum.GetValues(typeof(Enumerations.PowerUps)));
		var random = new Random();
		powerUpList.Sort((x, y) => random.Next(-1, 2));
		powerUpList.ForEach(x => result.Push(x));
		return result;
	}

	public void AddPowerUp() 
	{
		if (CurrentPowerUps.Count == POWER_UP_LIMIT) CurrentPowerUps.Dequeue();

		Enumerations.PowerUps? newPU = null;
		do 
		{
			if (AvailablePowerUps.Count == 0) AvailablePowerUps = InitPowerUpStack();
			var pulledPU = AvailablePowerUps.Pop();
			if (!CurrentPowerUps.Contains(pulledPU)) newPU = pulledPU;
		} while (!newPU.HasValue);

		GD.Print($"Pulled power up {newPU.ToString()}.");
		CurrentPowerUps.Enqueue(newPU.Value);
	}
}

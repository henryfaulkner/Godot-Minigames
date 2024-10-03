using System;
using System.Collections.Generic;

public class PowerUpService
{
	private const int POWER_UP_LIMIT = 2;

	public Stack<Enumerations.PowerUps> GetPowerUpStack()
	{
		var result = new Stack<Enumerations.PowerUps>();
		var powerUpList = new List<Enumerations.PowerUps>((Enumerations.PowerUps[])Enum.GetValues(typeof(Enumerations.PowerUps)));
		var random = new Random();
		powerUpList.Sort((x, y) => random.Next(-1, 2));
		powerUpList.ForEach(x => result.Push(x));
		return result;
	}

	public void AddPowerUp(Queue<Enumerations.PowerUps> PlayersCurrentPowerUps, Stack<Enumerations.PowerUps> PlayersAvailablePowerUps) 
	{
		if (PlayersCurrentPowerUps.Count == POWER_UP_LIMIT) PlayersCurrentPowerUps.Dequeue();

		Enumerations.PowerUps? newPU = null;
		do 
		{
			if (PlayersAvailablePowerUps.Count == 0) PlayersAvailablePowerUps = GetPowerUpStack();
			var pulledPU = PlayersAvailablePowerUps.Pop();
			if (!PlayersCurrentPowerUps.Contains(pulledPU)) newPU = pulledPU;
		} while (!newPU.HasValue);

		PlayersCurrentPowerUps.Enqueue(newPU.Value);
	}
}

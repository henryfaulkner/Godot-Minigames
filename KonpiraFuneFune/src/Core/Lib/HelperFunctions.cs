using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;
using System.Threading;

public static class HelperFunctions
{
	public static void SetTimeout(Action action, int delay)
	{
		var cancellationTokenSource = new CancellationTokenSource();
		var cancellationToken = cancellationTokenSource.Token;

		Task.Delay(delay).ContinueWith(async (t) =>
		{
			action();
		}, cancellationToken);
	}
}

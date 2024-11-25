using Godot;
using System;
using System.Threading.Tasks;

public class FeedCommand : Command
{
	public override async Task<bool> ExecuteAsync(Enumerations.Commands command)
	{
		var observables = new Observables();
		observables.EmitFeedPressed();
		return true;
	}
}

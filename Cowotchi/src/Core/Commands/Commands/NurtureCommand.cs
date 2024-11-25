using Godot;
using System;
using System.Threading.Tasks;

public class NurtureCommand : Command
{    
	public override async Task<bool> ExecuteAsync(Enumerations.Commands command)
	{
		var observables = new Observables();
		observables.EmitNurturePressed();
		return true;
	}
}

using Godot;
using System.Threading.Tasks;

public abstract class Command : Node
{
	public abstract Task<bool> ExecuteAsync(Enumerations.Commands command);
}

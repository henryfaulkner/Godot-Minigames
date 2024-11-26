using Godot;
using System.Threading.Tasks;

public abstract partial class Command : Node
{
	public abstract Task<bool> ExecuteAsync(Enumerations.Commands command);
}

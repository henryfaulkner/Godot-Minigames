using System.Threading.Tasks;

public interface ICommander
{
	CommandInvoker _invoker { get; set; }
	Task<bool> ExecuteCommandAsync(Enumerations.Commands command);
}

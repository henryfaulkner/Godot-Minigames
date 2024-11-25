using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public class CommandInvoker
{
	private readonly Dictionary<Enumerations.Commands, Command> _commands = new();

	private readonly ILoggerService _logger;

	public CommandInvoker(ILoggerService logger)
	{
		_logger = logger;
	}

	// Register a command with a key
	public void RegisterCommand(Enumerations.Commands commandKey, Command command)
	{
		_commands[commandKey] = command;
	}

	// Execute a command based on its key
	public async Task<bool> ExecuteCommandAsync(Enumerations.Commands commandKey)
	{
		if (_commands.TryGetValue(commandKey, out var command))
		{
			return await command.Execute();
		}
		else
		{
			_logger.LogError($"Command '{actionName.GetDescription()}' not found.");
		}
		return false;
	}

	// Optional: Clear or list commands
	public void ClearCommands() => _commands.Clear();
}

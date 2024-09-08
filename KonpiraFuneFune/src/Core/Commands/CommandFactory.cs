public static class CommandFactory
{
	public static TapCommand CreateTapCommand(AbstractCommander commander, CommandService serviceCommand)
	{
		var result = new TapCommand(commander, serviceCommand);
		return result;
	}

	public static GrabCommand CreateGrabCommand(AbstractCommander commander, CommandService serviceCommand)
	{
		var result = new GrabCommand(commander, serviceCommand);
		return result;
	}

	public static KnockCommand CreateKnockCommand(AbstractCommander commander, CommandService serviceCommand)
	{
		var result = new KnockCommand(commander, serviceCommand);
		return result;
	}
}

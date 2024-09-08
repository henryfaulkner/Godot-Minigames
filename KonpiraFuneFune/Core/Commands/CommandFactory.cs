public static class CommandFactory
{
    public static TapCommand CreateTapCommand()
    {
        var result = new TapCommand();
        return result;
    }

    public static GrabCommand CreateGrabCommand()
    {
        var result = new GrabCommand();
        return result;
    }

    public static KnockCommand CreateKnockCommand()
    {
        var result = new KnockCommand();
        return result;
    }
}
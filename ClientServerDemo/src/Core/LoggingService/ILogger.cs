public interface ILogger
{
    public void Log(Enumerations.LogLevels logLevel, string message);
    private void PublishLog(string message);
}
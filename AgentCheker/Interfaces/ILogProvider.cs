namespace AgentChecker.Interfaces
{
    using AgentChecker.Log;

    public interface ILogProvider
    {
        void WriteIntoFile(Logger logger);
    }
}

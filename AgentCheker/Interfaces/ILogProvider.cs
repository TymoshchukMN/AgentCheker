namespace AgentCheker.Interfaces
{
    using AgentCheker.Log;

    public interface ILogProvider
    {
        void WriteIntoFile(Logger logger);
    }
}

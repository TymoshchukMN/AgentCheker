namespace AgentCheker.Interfaces
{
    using AgentCheker.Log;

    public interface IDataProvider
    {
        void WriteIntoFile(Logger logger);
    }
}

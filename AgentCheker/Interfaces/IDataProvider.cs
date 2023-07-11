namespace AgentCheker.Interfaces
{
    using AgentCheker.Logger;

    public interface IDataProvider
    {
        void WriteIntoFile(Logger logger);
    }
}

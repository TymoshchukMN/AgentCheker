using AgentCheker.DataBase;
using AgentCheker.Log;
using AgentCheker.Mail;

namespace AgentCheker.Interfaces
{
    public interface IDataProvider
    {
        string Query { get; }

        string ConnectionString { get; }

        string ServerName { get; }
    }
}

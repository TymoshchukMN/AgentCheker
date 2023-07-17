using System.Collections.Generic;
using AgentCheker.DataBase;
using AgentCheker.Log;
using AgentCheker.Mail;

namespace AgentCheker.Interfaces
{
    public interface IDataProvider
    {
        List<string> GetPC(Logger logger, Email email);
    }
}

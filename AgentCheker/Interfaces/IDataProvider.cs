using System.Collections.Generic;
using AgentChecker.DataBase;
using AgentChecker.Log;
using AgentChecker.Mail;

namespace AgentChecker.Interfaces
{
    public interface IDataProvider
    {
        List<PC> GetPC(
            Logger logger,
            Email email,
            List<PC> notConnected);
    }
}

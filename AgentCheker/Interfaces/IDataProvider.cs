using System.Collections.Generic;
using AgentChecker.DataBase;
using AgentChecker.Log;
using AgentChecker.Mail;
using FiredProcessing;

namespace AgentChecker.Interfaces
{
    public interface IDataProvider
    {
        void GetPC(
            Logger logger,
            Email email,
            LDAP ldap);
    }
}

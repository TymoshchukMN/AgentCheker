using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AgentCheker.Log;
using AgentCheker.Log.Enums;
using AgentCheker.Mail;

namespace AgentCheker.DataBase
{
    public class DateBaseDC : DateBase
    {
        #region CTORs

        public DateBaseDC(
            string server,
            string userName,
            string dataBase,
            int port,
            string pass)
            : base(server, userName, dataBase, port, pass)
        {
            ServerName = server;
            ConnectionString = string.Format($"Server = {server},{port}; Database = {dataBase}; User Id = aku0\\{userName}; Password = {pass}");
            Query = @"SELECT comp.FQDN_NAME,
		            Dateadd(s, convert(bigint, agent.[LAST_CONTACT_TIME]) / 1000
			                    , convert(datetime, '1-1-1970'))+ '03:00:00' as 'lastConnectTime'
                    from [desktopcentral].[dbo].[ManagedComputer] as comp
                    inner join [desktopcentral].[dbo].[AgentContact] as agent on agent.RESOURCE_ID = comp.RESOURCE_ID
                    order by agent.[LAST_CONTACT_TIME] desc
                    ";
        }

        #endregion CTORs

        #region PROPERTIES

        public override string Query
        {
            get { return Query; }
        }

        #endregion PROPERTIES

    }
}

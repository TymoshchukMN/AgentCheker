namespace AgentCheker.DataBase
{
    using AgentCheker.DataBase.Enums;

    public class DateBaseDC : DateBase
    {
        #region CTORs

        public DateBaseDC(
            string server,
            string userName,
            string dataBase,
            int port,
            string pass)
            : base(server, userName, dataBase, port, pass, ServerDB.DC)
        {
            ServerName = server;
            ConnectionString = $"Server={server};Database={dataBase};User Id={userName};Password={pass};Trusted_Connection=False;MultipleActiveResultSets=true";

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

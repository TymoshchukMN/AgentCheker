using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AgentCheker.DataBase.Enums;
using AgentCheker.Interfaces;
using AgentCheker.Log;
using AgentCheker.Log.Enums;
using AgentCheker.Mail;

namespace AgentCheker.DataBase
{
    public class DateBase : IDataProvider
    {
        #region FIELDS

        private const string _dcQuery = @"SELECT comp.FQDN_NAME,
		            Dateadd(s, convert(bigint, agent.[LAST_CONTACT_TIME]) / 1000
			                    , convert(datetime, '1-1-1970'))+ '03:00:00' as 'lastConnectTime'
                    from [desktopcentral].[dbo].[ManagedComputer] as comp
                    inner join [desktopcentral].[dbo].[AgentContact] as agent on agent.RESOURCE_ID = comp.RESOURCE_ID
                    order by agent.[LAST_CONTACT_TIME] desc
                    ";

        private const string _esetQuery = @"
                        select	t1.computer_name,
                            t2.[computer_connected] + '03:00:00' as 'lastConnected'
                        from tbl_computers as t1
                        inner join [era_db].[dbo].[tbl_computers_aggr] as t2
                        on t1.computer_id = t2.computer_id
                        where t2.[computer_connected] is not NULL 
                        and t2.[computer_connected] <= DATEADD(DAY,-14,GETDATE())
                        order by t2.[computer_connected] desc
                        ";

        private readonly ServerDB _dbServer;
        private string _query;
        private string _connectionString;
        private string _serverName;

        #endregion FIELDS

        #region CTORs

        public DateBase(
            string server,
            string userName,
            string dataBase,
            int port,
            string pass,
            ServerDB dbServer)
        {
            _serverName = server;
            _connectionString = string.Format($"Server = {server},{port}; Database = {dataBase}; User Id = aku0\\{userName}; Password = {pass}");
            _dbServer = dbServer;
        }

        #endregion CTORs

        #region PROPERTIES

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }

            set
            {
                _connectionString = value;
            }
        }

        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        public virtual string Query
        {
            get
            {
                return _query;
            }

            set => _query = value;
        }

        public string DCquery
        {
            get { return _dcQuery; }
        }

        public string EsetQuery
        {
            get { return _esetQuery; }
        }

        #endregion PROPERTIES

        public List<string> GetPC(Logger logger, Email email)
        {
            List<string> pc = new List<string>();

            using (var con = new SqlConnection(ConnectionString))
            {
                try
                {
                    string message = $"{DateTime.Now};{MessageType.Info}" +
                                   $": Attempt to connected to Database {ServerName}";

                    logger.AddLog(message);

                    con.Open();

                    message = $"{DateTime.Now};{MessageType.Info}" +
                                    $": Connected to Database {ServerName}";

                    logger.AddLog(message);
                }
                catch (Exception ex)
                {
                    string message = $"{DateTime.Now};{MessageType.Error}" +
                                $": Action failed by a reason; Action got this " +
                                $"Error. Cannont connect to DB\n{ex.Message}";

                    logger.AddLog(message);
                    email.SendMail(message);

                    throw;
                }

                SqlCommand command;
                SqlDataReader dataReader;

                switch (_dbServer)
                {
                    case ServerDB.DC:
                        _query = DCquery;
                        break;
                    case ServerDB.Eset:
                        _query = EsetQuery;
                        break;
                    default:
                        break;
                }

                command = new SqlCommand(Query, con);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2));
                }

                dataReader.Close();
                command.Dispose();
                con.Close();
            }

            return pc;
        }
    }
}

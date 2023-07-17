﻿using System;
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

        private const string _dcQuery = @"SELECT comp.FQDN_NAME  as 'pcName',
		            Dateadd(s, convert(bigint, agent.[LAST_CONTACT_TIME]) / 1000
			                    , convert(datetime, '1-1-1970'))+ '03:00:00' as 'lastConnectTime'
                    from [desktopcentral].[dbo].[ManagedComputer] as comp
                    inner join [desktopcentral].[dbo].[AgentContact] as agent on agent.RESOURCE_ID = comp.RESOURCE_ID
                    order by agent.[LAST_CONTACT_TIME] desc
                    ";

        private const string _esetQuery = @"
                        select	t1.computer_name as 'pcName',
                            t2.[computer_connected] + '03:00:00' as 'lastConnectTime'
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
            string serverName,
            ServerDB dbServer)
        {
            _serverName = serverName;
            _dbServer = dbServer;
        }

        public DateBase(
            string server,
            string userName,
            string dataBase,
            int port,
            string pass,
            ServerDB serverDB)
        {
            ServerName = server;
            ConnectionString = $"Server={server};" +
                $"Database={dataBase};" +
                $"User Id={userName};" +
                $"Password={pass};" +
                $"Trusted_Connection=False;" +
                $"MultipleActiveResultSets=true";
            _dbServer = serverDB;
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

        public List<string> GetPC(Logger logger, Email email, List<string> notConnected)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                string message;
                try
                {
                    message = $"{DateTime.Now};{MessageType.Info}" +
                                   $":\tAttempt to connected to Database {ServerName}";

                    logger.AddLog(message);

                    connection.Open();

                    message = $"{DateTime.Now};\t{MessageType.Info}" +
                                    $":\tConnected to Database {ServerName}";

                    logger.AddLog(message);
                }
                catch (Exception ex)
                {
                    message = $"{DateTime.Now};\t{MessageType.Error}" +
                                $":\tAction failed by a reason; Action got this " +
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

                command = new SqlCommand(Query, connection);
                dataReader = command.ExecuteReader();
                bool isFinded = false;

                while (dataReader.Read())
                {
                    if (!isFinded)
                    {
                        isFinded = true;
                        message = $"Найдены устройства на сервере {ServerName}{(char)10}";
                        logger.AddLog(message);
                    }

                    notConnected.Add(dataReader["pcName"] + ":" + dataReader["lastConnectTime"].ToString());

                    message = $"{DateTime.Now};\t{MessageType.Info}" +
                                $": {dataReader["pcName"]}\t{dataReader["lastConnectTime"]}";
                    UI.PrintLog(message);

                    logger.AddLog(message);
                }

                dataReader.Close();
                command.Dispose();
                connection.Close();
            }

            return notConnected;
        }
    }
}

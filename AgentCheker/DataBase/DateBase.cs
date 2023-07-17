using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using AgentCheker.Interfaces;
using AgentCheker.Log;
using AgentCheker.Log.Enums;
using AgentCheker.Mail;

namespace AgentCheker.DataBase
{
    public class DateBase : IDataProvider
    {
        #region FIELDS

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
            string pass)
        {
            _serverName = server;
            _connectionString = string.Format($"Server = {server},{port}; Database = {dataBase}; User Id = aku0\\{userName}; Password = {pass}");
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

        #endregion PROPERTIES

        public List<string> GetPC(Logger logger, Email email)
        {
            List<string> pc = new List<string>();

            using (var con = new SqlConnection(ConnectionString))
            {
                try
                {
                    con.Open();

                    string message = $"{DateTime.Now};{MessageType.Info}" +
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

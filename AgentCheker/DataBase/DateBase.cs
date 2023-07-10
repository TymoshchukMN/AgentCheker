using System;
using System.Collections.Generic;
using System.Data;

namespace AgentCheker.DataBase
{
    internal class DateBase
    {
        #region FIELDS

        private string _connectionString;

        #endregion FIELDS

        #region CTORs

        public DateBase(
            string server,
            string userName,
            string dataBase,
            int port,
            string pass)
        {
            _connectionString = string.Format(
                    $"Server={server};" +
                    $"Username={userName};" +
                    $"Database={dataBase};" +
                    $"Port={port};" +
                    $"Password={pass}");
        }

        #endregion CTORs

        #region PROPERTIES

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        #endregion PROPERTIES

    }
}

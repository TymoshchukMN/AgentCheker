using AgentCheker.Interfaces;

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

    }
}

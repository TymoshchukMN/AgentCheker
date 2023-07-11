namespace AgentCheker.Logger
{
    /// <summary>
    /// Logger class.
    /// </summary>
    public class Logger
    {
        private const ushort ArrayLogSize = 200;
        private static Logger _logger;
        private string[] _logs;

        /// <summary>
        /// Index for addisng new log string ti _logs.
        /// </summary>
        private ushort _logIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// Def ctor.
        /// </summary>
        private Logger()
        {
            _logs = new string[ArrayLogSize];
        }

        /// <summary>
        /// Gets get data logs.
        /// </summary
        public string[] Logs
        {
            get
            {
                return _logs;
            }
        }

        /// <summary>
        /// Creation singletone class.
        /// </summary>
        /// <returns>
        /// Logger object.
        /// </returns>
        public static Logger GetInstatce()
        {
            if (_logger == null)
            {
                _logger = new Logger();
            }

            return _logger;
        }

        /// <summary>
        /// Add new log.
        /// </summary>
        /// <param name="newLog">
        /// New logging event.
        /// </param>
        public void AddLog(string newLog)
        {
            _logs[_logIndex] = newLog;
            _logIndex++;
        }
    }
}

namespace AgentChecker.Json
{
    using AgentChecker.DataBase;
    using AgentChecker.Mail;

    public class Config
    {
        public DBConfig DBConfig { get; set; }

        public MailConfig MailConfig { get; set; }

        public LoggerConfig LoggerConfig { get; set; }
    }
}

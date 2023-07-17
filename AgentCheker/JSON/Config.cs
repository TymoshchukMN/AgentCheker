namespace AgentCheker.Json
{
    using AgentCheker.DataBase;
    using AgentCheker.Mail;

    public class Config
    {
        public DBConfig DBConfig { get; set; }

        public MailConfig MailConfig { get; set; }

        public LoggerConfig LoggerConfig { get; set; }
    }
}

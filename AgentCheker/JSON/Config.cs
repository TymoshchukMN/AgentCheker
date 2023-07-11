namespace AgentCheker.Json
{
    using AgentCheker.DataBase;
    using AgentCheker.Logger;
    using AgentCheker.Mail;

    public class Config
    {
        public DBConfig DataBaseConfig { get; set; }

        public MailConfig MailConfig { get; set; }

        public LoggerConfig LoggerConfig { get; set; }
    }
}

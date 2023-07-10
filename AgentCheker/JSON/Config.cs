using AgentCheker.DataBase;
using AgentCheker.Mail;

namespace AgentCheker.Json
{
    public class Config
    {
        public DBConfig DataBaseConfig { get; set; }

        public MailConfig MailConfig { get; set; }
    }
}

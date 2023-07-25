namespace AgentChecker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AgentChecker.DataBase;
    using AgentChecker.DataBase.Enums;
    using AgentChecker.Encription;
    using AgentChecker.Json;
    using AgentChecker.Log;
    using AgentChecker.Mail;
    using AgentChecker;
    using Newtonsoft.Json;

    public class Starter
    {
        public static void Run()
        {
            Logger logger = Logger.GetInstatce();

            const string MAIL_CONF_FILE_PATH = "N:\\Personal\\TymoshchukMN\\" +
                "AgentCheker\\MailConfigFile.json";

            string configFileMail = File.ReadAllText(MAIL_CONF_FILE_PATH);
            Config configJSONmail =
                JsonConvert.DeserializeObject<Config>(configFileMail);

            Email email = new Email(
               configJSONmail.MailConfig.FromAddress,
               configJSONmail.MailConfig.ToAddress,
               configJSONmail.MailConfig.MailServer,
               Decrypt.DecryptCipherTextToPlainText(
                   configJSONmail.MailConfig.FromPass),
               configJSONmail.MailConfig.Port);

            const string DC_CONF_FILE_PATH = "N:\\Personal\\TymoshchukMN\\" +
                "AgentCheker\\DBconfigFileDC.json";

            string configFileDC = File.ReadAllText(DC_CONF_FILE_PATH);
            Config configJSONdesckCen
                = JsonConvert.DeserializeObject<Config>(configFileDC);

            DateBase deskCenDB = new DateBase(
                configJSONdesckCen.DBConfig.Server,
                configJSONdesckCen.DBConfig.UserName,
                configJSONdesckCen.DBConfig.DBname,
                configJSONdesckCen.DBConfig.Port,
                Decrypt.DecryptCipherTextToPlainText(
                    configJSONdesckCen.DBConfig.Pass),
                ServerDB.DC);

            List<PC> dcAlldevices = new List<PC>();
            LDAP ldap = new LDAP();
            deskCenDB.GetPC(logger, email, ldap);
            dcAlldevices = deskCenDB.NotConnectedPC;

            List<PC> dcNotConnected =
                dcAlldevices.Where(x => x.LastConnectionTime <
                DateTime.Today.AddDays(-14)).ToList();

            const string Eset_Conf_File_Path = "N:\\Personal\\TymoshchukMN\\" +
              "AgentCheker\\DBconfigFileEset.json";

            string configFileEset = File.ReadAllText(Eset_Conf_File_Path);
            Config configJsonEset =
                JsonConvert.DeserializeObject<Config>(configFileEset);

            DateBase esetDB = new DateBase(
                configJsonEset.DBConfig.Server,
                configJsonEset.DBConfig.UserName,
                configJsonEset.DBConfig.DBname,
                configJsonEset.DBConfig.Port,
                Decrypt.DecryptCipherTextToPlainText(
                    configJsonEset.DBConfig.Pass),
                ServerDB.Eset);

            List<PC> esetNotConnected = new List<PC>();

            esetDB.GetPC(logger, email, ldap);
            Checker checker = new Checker();
            esetNotConnected = esetDB.NotConnectedPC;

            List<PC> dcPingResult = new List<PC>();
            List<PC> esetPingResult = new List<PC>();

            checker.CheckPCs(
                dcNotConnected,
                esetNotConnected,
                ref dcPingResult,
                ref esetPingResult);

            email.ProcessEmailBody(deskCenDB.ServerName, dcPingResult);
            email.ProcessEmailBody(esetDB.ServerName, esetPingResult);
            email.SendMail();

            Console.ReadLine();
        }
    }
}
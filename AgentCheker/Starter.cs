namespace AgentCheker
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AgentCheker.DataBase;
    using AgentCheker.DataBase.Enums;
    using AgentCheker.Encription;
    using AgentCheker.Json;
    using AgentCheker.Log;
    using AgentCheker.Mail;
    using Newtonsoft.Json;

    public class Starter
    {
        public static void Run()
        {
            Logger logger = Logger.GetInstatce();

            const string MAIL_Conf_File_Path = "N:\\Personal\\TymoshchukMN\\" +
                "AgentCheker\\MailConfigFile.json";

            string configFileMail = File.ReadAllText(MAIL_Conf_File_Path);
            Config configJSONmail = JsonConvert.DeserializeObject<Config>(configFileMail);

            Email email = new Email(
               configJSONmail.MailConfig.FromAddress,
               configJSONmail.MailConfig.ToAddress,
               configJSONmail.MailConfig.MailServer,
               Decrypt.DecryptCipherTextToPlainText(configJSONmail.MailConfig.FromPass),
               configJSONmail.MailConfig.Port);

            const string DC_Conf_File_Path = "N:\\Personal\\TymoshchukMN\\" +
                "AgentCheker\\DBconfigFileDC.json";

            string configFileDC = File.ReadAllText(DC_Conf_File_Path);
            Config configJSONdesckCen = JsonConvert.DeserializeObject<Config>(configFileDC);

            DateBase deskCenDB = new DateBase(
                configJSONdesckCen.DBConfig.Server,
                configJSONdesckCen.DBConfig.UserName,
                configJSONdesckCen.DBConfig.DBname,
                configJSONdesckCen.DBConfig.Port,
                Decrypt.DecryptCipherTextToPlainText(
                    configJSONdesckCen.DBConfig.Pass),
                ServerDB.DC);

            // List<string> dcNotConnected = new List<string>();

            var dcNotConnected = new List<PC>();

            deskCenDB.GetPC(logger, email, dcNotConnected);

            var a = dcNotConnected.Where(x => x.LastConnectionTime < DateTime.Today.AddDays(-7)).ToList();
            const string Eset_Conf_File_Path = "N:\\Personal\\TymoshchukMN\\" +
              "AgentCheker\\DBconfigFileEset.json";

            string configFileEset = File.ReadAllText(Eset_Conf_File_Path);
            Config configJsonEset = JsonConvert.DeserializeObject<Config>(configFileEset);

            DateBase esetDB = new DateBase(
                configJsonEset.DBConfig.Server,
                configJsonEset.DBConfig.UserName,
                configJsonEset.DBConfig.DBname,
                configJsonEset.DBConfig.Port,
                Decrypt.DecryptCipherTextToPlainText(
                    configJsonEset.DBConfig.Pass),
                ServerDB.Eset);

            List<PC> esetNotConnected = new List<PC>();
            esetDB.GetPC(logger, email, esetNotConnected);

            Console.ReadLine();
        }
    }
}
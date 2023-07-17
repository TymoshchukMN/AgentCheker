namespace AgentCheker
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using AgentCheker.DataBase;
    using AgentCheker.Encription;
    using AgentCheker.Interfaces;
    using AgentCheker.Json;
    using AgentCheker.Log;
    using AgentCheker.Log.Enums;
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

            const string Eset_Conf_File_Path = "N:\\Personal\\TymoshchukMN\\" +
              "AgentCheker\\DBconfigFileEset.json";

            string configFileEset = File.ReadAllText(Eset_Conf_File_Path);
            Config configJsonEset = JsonConvert.DeserializeObject<Config>(configFileEset);

            DateBaseEset esetDB = new DateBaseEset(
                configJsonEset.DBConfig.Server,
                configJsonEset.DBConfig.UserName,
                configJsonEset.DBConfig.DBname,
                configJsonEset.DBConfig.Port,
                Decrypt.DecryptCipherTextToPlainText(
                    configJsonEset.DBConfig.Pass));

            esetDB.GetPC(logger, email);

            const string DC_Conf_File_Path = "N:\\Personal\\TymoshchukMN\\" +
                "AgentCheker\\DBconfigFileDC.json";

            string configFileDC = File.ReadAllText(DC_Conf_File_Path);
            Config configJSONdesckCen = JsonConvert.DeserializeObject<Config>(configFileDC);

            DateBaseDC deskCenDB = new DateBaseDC(
                configJSONdesckCen.DBConfig.Server,
                configJSONdesckCen.DBConfig.UserName,
                configJSONdesckCen.DBConfig.DBname,
                configJSONdesckCen.DBConfig.Port,
                Decrypt.DecryptCipherTextToPlainText(
                    configJSONdesckCen.DBConfig.Pass));

            List<string> dcNotConnected = new List<string>();
        }
    }
}
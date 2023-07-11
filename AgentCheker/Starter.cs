namespace AgentCheker
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using AgentCheker.DataBase;
    using AgentCheker.Encription;
    using AgentCheker.Json;
    using AgentCheker.Log;
    using AgentCheker.Mail;
    using Newtonsoft.Json;

    internal static class Starter
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
                configJSONdesckCen.DataBaseConfig.Server,
                configJSONdesckCen.DataBaseConfig.UserName,
                configJSONdesckCen.DataBaseConfig.DBname,
                configJSONdesckCen.DataBaseConfig.Port,
                Decrypt.DecryptCipherTextToPlainText(
                    configJSONdesckCen.DataBaseConfig.Pass));

            using (var con = new SqlConnection(deskCenDB.ConnectionString))
            {
                try
                {
                    con.Open();
                }
                catch (Exception ex)
                {
                    string message = string.Format(
                       $"Error. Cannont connect to DB\n{ex.Message}");

                    email.SendMail(message);

                    throw;
                }
            }

            const string Eset_Conf_File_Path = "N:\\Personal\\TymoshchukMN\\" +
              "AgentCheker\\DBconfigFileEset.json";

            string configFileEset = File.ReadAllText(Eset_Conf_File_Path);
            Config configJSONEset = JsonConvert.DeserializeObject<Config>(configFileEset);

            DateBase esetDB = new DateBase(
                configJSONEset.DataBaseConfig.Server,
                configJSONEset.DataBaseConfig.UserName,
                configJSONEset.DataBaseConfig.DBname,
                configJSONEset.DataBaseConfig.Port,
                Decrypt.DecryptCipherTextToPlainText(
                    configJSONEset.DataBaseConfig.Pass));

            List<string> dcNotConnected = new List<string>();

        }
    }
}

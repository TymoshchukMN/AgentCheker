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

            GetPC(logger, email, esetDB);

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

        private static void GetPC(Logger logger, Email email, DateBaseEset dateBase)
        {
            using (var con = new SqlConnection(dateBase.ConnectionString))
            {
                try
                {
                    con.Open();

                    string message = $"{DateTime.Now};{MessageType.Info}" +
                                    $": Connected to Database {dateBase.ServerName}";

                    logger.AddLog(message);
                }
                catch (Exception ex)
                {
                    string message = $"{DateTime.Now};{MessageType.Error}" +
                                $": Action failed by a reason; Action got this " +
                                $"Error. Cannont connect to DB\n{ex.Message}";

                    logger.AddLog(message);
                    email.SendMail(message);

                    throw;
                }

                SqlCommand command;
                SqlDataReader dataReader;
                string sqlQuery = @"
                        select	t1.computer_name,
                            t2.[computer_connected] + '03:00:00' as 'lastConnected'
                        from tbl_computers as t1
                        inner join [era_db].[dbo].[tbl_computers_aggr] as t2
                        on t1.computer_id = t2.computer_id
                        where t2.[computer_connected] is not NULL 
                        and t2.[computer_connected] <= DATEADD(DAY,-14,GETDATE())
                        order by t2.[computer_connected] desc
                        ";

                command = new SqlCommand(sqlQuery, con);
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2));
                }

                dataReader.Close();
                command.Dispose();
                con.Close();
            }
        }
    }
}
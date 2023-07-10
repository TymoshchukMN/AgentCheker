using System.IO;
using AgentCheker.DataBase;
using AgentCheker.Encription;
using AgentCheker.Json;
using Newtonsoft.Json;

namespace AgentCheker
{
    internal static class Starter
    {
        public static void Run()
        {
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


        }
    }
}

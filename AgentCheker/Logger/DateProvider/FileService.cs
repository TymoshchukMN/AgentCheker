namespace AgentCheker.Logger.DateProvider
{
    using System;
    using System.IO;
    using AgentCheker.Interfaces;
    using AgentCheker.Json;
    using AgentCheker.Logger;
    using Newtonsoft.Json;

    public class FileService : IDataProvider
    {
        public void WriteIntoFile(Logger logger)
        {
            const string Logger_Config_File_Path = "N:\\Personal\\TymoshchukMN\\" +
                "AgentCheker\\ConfigLogFile.json";

            var loggerConfigFile = File.ReadAllText(Logger_Config_File_Path);
            var loggerConfigJSON =
                JsonConvert.DeserializeObject<Config>(loggerConfigFile);

            string path = $"{loggerConfigJSON.LoggerConfig.DirectoryPath}" +
                $"\\{DateTime.Now.ToString("hh.mm.ss dd.MM.yyyy")}" +
                $"{loggerConfigJSON.LoggerConfig.FileExtension}";

            File.WriteAllText(
                path,
                string.Join(
                    loggerConfigJSON.LoggerConfig.LineSeparator.ToString(),
                    logger.Logs));
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using AgentChecker.DataBase;
using AgentChecker.Log;
using AgentChecker.Log.Enums;
using AgentCheker.Interfaces;

namespace AgentChecker
{
    public class Checker : IChecker
    {
        public static bool PingHost(string nameOrAddress, DateTime lasconnect)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;

                string message = $"{DateTime.Now};\t{MessageType.Success}" +
                                $": {nameOrAddress}\tping success\t{lasconnect}";
                UI.PrintLog(message);
            }
            catch
            {
                Logger logger = Logger.GetInstatce();

                string message = $"{DateTime.Now};\t{MessageType.Error}" +
                                $": {nameOrAddress}\tCann`t ping host\t{lasconnect}";
                logger.AddLog(message);
                UI.PrintLog(message);
            }
            finally
            {
                pinger?.Dispose();
            }

            return pingable;
        }

        public void CheckPCs(
            List<PC> dcPCs,
            List<PC> esetPCs,
            ref List<PC> dcPingResult,
            ref List<PC> esetPingResult)
        {
            Console.WriteLine("=======  DC ==============");
            dcPingResult = dcPCs.Skip(1).Where((x) =>
            {
                return PingHost(x.PcName, x.LastConnectionTime);
            }).Select((n) => n).ToList();

            Console.WriteLine("=======  Eset ==============");

            esetPingResult = esetPCs.Skip(1).Where((x) =>
            {
                return PingHost(x.PcName, x.LastConnectionTime);
            }).Select((n) => n).ToList();
        }
    }
}

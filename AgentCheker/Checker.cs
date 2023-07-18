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
            List<PC> dcPingResult,
            List<PC> esetPingResult)
        {
            Console.WriteLine("=======  DC ==============");
            var availablePCfromDC = dcPCs.Where((x) =>
            {
                return PingHost(x.PcName, x.LastConnectionTime);
            }).Select((n) => (n.PcName, n.LastConnectionTime)).ToList();

            dcPingResult = (List<PC>)(IEnumerable)availablePCfromDC.ToList();

            foreach (var pc in availablePCfromDC)
            {
                UI.PrintLog($"{DateTime.Now};\t{MessageType.Success}" +
                              $": {pc.PcName}\t{pc.LastConnectionTime}");
            }

            Console.WriteLine("=======  Eset ==============");

            var availablePCfromEset = esetPCs.Where((x) =>
            {
                return PingHost(x.PcName, x.LastConnectionTime);
            }).Select((n) => (n.PcName, n.LastConnectionTime));

            esetPingResult = (List<PC>)(IEnumerable)availablePCfromEset.ToList();

            foreach (var pc in availablePCfromEset)
            {
                UI.PrintLog($"{DateTime.Now};\t{MessageType.Success}" +
                              $": {pc.PcName}\t{pc.LastConnectionTime}");
            }
        }
    }
}

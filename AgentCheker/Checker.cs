using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgentChecker.DataBase;
using AgentChecker.Log;
using AgentChecker.Log.Enums;
using AgentCheker.Interfaces;

namespace AgentChecker
{
    public class Checker : IChecker
    {
        public static bool PingHost(string nameOrAddress)
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
                                $": {nameOrAddress}\tCann`t ping host";
                logger.AddLog(message);
                UI.PrintLog(message);
            }
            finally
            {
                pinger?.Dispose();
            }

            return pingable;
        }

        public List<PC> CheckPCs(List<PC> dcPCs, List<PC> esetPCs)
        {

            var ll = dcPCs.Where((x) =>
            {
                return PingHost(x.PcName);
            }).Select((n) => (n.PcName, n.LastConnectionTime));

            foreach (var pc in ll)
            {
                Console.WriteLine($"{pc.PcName}\t{pc.LastConnectionTime}");
            }

            return (List<PC>)ll;
        }
    }
}

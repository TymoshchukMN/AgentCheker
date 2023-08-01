using System;
using System.Net.NetworkInformation;
using AgentChecker.Log.Enums;

namespace AgentChecker
{
    internal class UI
    {
        public static void PrintLog(string log, IPStatus status)
        {
            string messageType = log.Split(';')[0];
            string pcName = log.Split(';')[1].Replace(".AKU.COM", string.Empty);
            string lastConnTime = log.Split(';')[2];

            MessageType ms = (MessageType)Enum.Parse(
                typeof(MessageType), messageType);

            switch (ms)
            {
                case MessageType.Error:
                    ChangeColor(messageType, ConsoleColor.Red);
                    break;
                case MessageType.Info:
                    ChangeColor(messageType, ConsoleColor.Cyan);
                    break;
                case MessageType.Warning:
                    ChangeColor(messageType, ConsoleColor.Yellow);
                    break;
                case MessageType.Success:
                    ChangeColor(messageType, ConsoleColor.Green);
                    break;
            }

            Console.Write($"\t{pcName}");

            if (status == IPStatus.Success)
            {
                Console.Write("\t");
                ChangeColor(messageType, ConsoleColor.Green);
            }
            else
            {
                Console.Write($"\t{status}");
            }

            Console.WriteLine($"\t\t\t{lastConnTime}");
        }

        public static void PrintErrPing(string log)
        {
            string messageType = log.Split(';')[0];
            string pcName = log.Split(';')[1].Replace(".AKU.COM", string.Empty);
            string lastConnTime = log.Split(';')[2];

            ChangeColor(messageType, ConsoleColor.Red);

            const string ErrMessage = "Doesn't exist in DNS";
            Console.WriteLine($"\t{pcName}\t{ErrMessage}\t{lastConnTime}");
        }

        public static void PrintBoard(string system)
        {
            Console.WriteLine($"\n==============  {system}  ==============");
        }

        private static void ChangeColor(string str, ConsoleColor color)
        {
            ConsoleColor defColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = defColor;
        }
    }
}

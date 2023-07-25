using System;
using System.Net.NetworkInformation;
using AgentChecker.Log.Enums;

namespace AgentChecker
{
    internal class UI
    {
        public static void PrintLog(string log)
        {
            Console.Write($"{log.Split(';')[0]}\t");

            MessageType ms = (MessageType)Enum.Parse(
                typeof(MessageType),
                log.Split(';')[1].Split(':')[0]);

            switch (ms)
            {
                case MessageType.Error:
                    ChangeColor("Error\t\t", ConsoleColor.Red);
                    break;
                case MessageType.Info:
                    ChangeColor("Info", ConsoleColor.Cyan);
                    break;
                case MessageType.Warning:
                    ChangeColor("Warning", ConsoleColor.Yellow);
                    break;
                case MessageType.Success:
                    ChangeColor("Success", ConsoleColor.Green);
                    break;
            }

            Console.WriteLine(
                $"{log.Split(';')[1].Substring(log.Split(';')[1].IndexOf(':'))}");
        }

        public static void PrintResPing(string log)
        {
            Console.Write($"{log.Split(';')[0]}\t");

            IPStatus ms = (IPStatus)Enum.Parse(
                typeof(IPStatus),
                log.Split(';')[1].Split(':')[0]);

            switch (ms)
            {
                case IPStatus.Success:
                    ChangeColor("Success\t\t", ConsoleColor.Green);
                    break;
                case IPStatus.DestinationHostUnreachable:
                    ChangeColor("Unreachable", ConsoleColor.Red);
                    break;
                case IPStatus.TimedOut:
                    ChangeColor("TimedOut", ConsoleColor.Red);
                    break;
                default:
                    ChangeColor($"{ms}", ConsoleColor.Red);
                    break;
            }

            Console.WriteLine(
                $"{log.Split(';')[1].Substring(log.Split(';')[1].IndexOf(':'))}");
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

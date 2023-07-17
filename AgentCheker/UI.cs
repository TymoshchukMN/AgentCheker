using System;
using AgentCheker.Log.Enums;

namespace AgentCheker
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
                    ChangeColor("Error", ConsoleColor.Red);
                    break;
                case MessageType.Info:
                    ChangeColor("Info", ConsoleColor.Cyan);
                    break;
                case MessageType.Warning:
                    ChangeColor("Warning", ConsoleColor.Yellow);
                    break;
            }

            Console.WriteLine(
                $"{log.Split(';')[1].Substring(log.Split(';')[1].IndexOf(':'))}");
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

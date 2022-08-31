using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welome to Zork!");
            string inputString = Console.ReadLine().Trim();
            Commands command = ToCommand(inputString);
            Console.WriteLine(command);
        }

        static Commands ToCommand(string commandString)
        {
            return Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Zork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welome to Zork!");

            bool isRunning = true;

            while (isRunning == true)
            {
                Console.WriteLine(_rooms[_currentRoom]);
                Console.Write("> ");
                string inputString = Console.ReadLine().Trim();
                Commands command = ToCommand(inputString);
                string outputString;
                switch (command)
                {
                    case Commands.Quit:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;
                    case Commands.Look:
                        outputString = "This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door.";
                        break;
                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        if (Move(command))
                        {
                            outputString = $"You moved {command}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    default:
                        outputString = "Unknown Command.";
                        break;
                }

                Console.WriteLine(outputString);
            }

        }

        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
        }

        private static bool Move(Commands command)
        {
            bool didMove = false;

            switch (command)
            {
                case Commands.North:
                case Commands.South:
                    break;
                case Commands.East when _currentRoom < _rooms.Length - 1:
                    _currentRoom++;
                    didMove = true;
                    break;

                case Commands.West when _currentRoom > 0:
                    _currentRoom--;
                    didMove = true;
                    break;
            }

            return didMove;
        }

        private static readonly string[] _rooms = { "Forest", "West of House", "Behind House", "Clearing", "Canyon View" };
        private static int _currentRoom = 1;
    }
}

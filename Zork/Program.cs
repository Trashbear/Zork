using System;
using System.Collections.Generic;

namespace Zork
{
    internal class Program
    {
        private static string CurrentRoom
        {
            get
            {
                return _rooms[Location.Row, Location.Column];
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welome to Zork!");

            bool isRunning = true;

            while (isRunning == true)
            {
                Console.WriteLine(CurrentRoom);
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
            Assert.IsTrue(IsDirection(command), "Invalid Direction.");

            bool didMove = true;

            switch (command)
            {
                case Commands.North when Location.Row < _rooms.GetLength(0) - 1:
                    Location.Row++;
                    break;

                case Commands.South when Location.Row > 0:
                    Location.Row--;
                    break;

                case Commands.East when Location.Column < _rooms.GetLength(1) - 1:
                    Location.Column++;
                    break;

                case Commands.West when Location.Column > 0:
                    Location.Column--;
                    break;

                default:
                    didMove = false;
                    break;
            }

            return didMove;
        }

        private static bool IsDirection(Commands command) => Directions.Contains(command);

        private static readonly string[,] _rooms =
        {
            { "Rocky Trail", "South of House", "Canyon View" },
            { "Forest", "West of House", "Behind House" },
            { "Dense Woods", "North of House", "Clearing" },

        };

        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.North,
            Commands.South,
            Commands.East,
            Commands.West
        };

        private static (int Row, int Column) Location = (1, 1);
    }
}

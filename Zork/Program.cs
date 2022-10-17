using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Zork
{
    internal class Program
    {
        private static Room CurrentRoom
        {
            get
            {
                return _rooms[Location.Row, Location.Column];
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Welome to Zork!");
            const string defaultRoomsFilename = "Rooms.json";
            string roomsFilename = (args.Length > 0 ? args[(int)CommandLineArguments.RoomsFilename] : defaultRoomsFilename);
            InitializeRooms(roomsFilename);
            Room previousRoom = null;
            bool isRunning = true;

            while (isRunning == true)
            {
                Console.WriteLine(CurrentRoom);
                if (previousRoom != CurrentRoom)
                {
                    Console.WriteLine(CurrentRoom.Description);
                    previousRoom = CurrentRoom;
                }
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
                        outputString = CurrentRoom.Description;
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

        private enum Fields
        {
            Name = 0,
            Description
        }

        private static void InitializeRooms(string roomsFilename) =>
            _rooms = JsonConvert.DeserializeObject<Room[,]>(File.ReadAllText(roomsFilename));

        private static Room[,] _rooms =
        {
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") },
        };

        private static readonly List<Commands> Directions = new List<Commands>
        {
            Commands.North,
            Commands.South,
            Commands.East,
            Commands.West
        };

        private static (int Row, int Column) Location = (1, 1);
        private enum CommandLineArguments
        {
            RoomsFilename = 0
        }
    }
}

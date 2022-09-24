﻿using System;
using System.Collections.Generic;

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

            InitializeRoomDescriptions();


            bool isRunning = true;

            while (isRunning == true)
            {
                Console.WriteLine(CurrentRoom);
                Console.Write(">");
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

        private static void InitializeRoomDescriptions()
        {
            _rooms[0, 0].Description = "You are on a rock-strewn trail.";                                                                                //Rocky Trail
            _rooms[0, 1].Description = "You are facing the south side of a white house. There is no door here, and all the windows are barred.";         //South of House
            _rooms[0, 2].Description = "You are at the top of the Great Canyon on its south wall.";                                                      //Canyon View

            _rooms[1, 0].Description = "This is a forest, with trees in all directions around you.";                                                    //Forest
            _rooms[1, 1].Description = "This is an open field west of a white house, with a boarded front door.";                                       //West of House
            _rooms[1, 2].Description = "You are behind the white house. In one corner of the house there is a small window which is slightly ajar.";    //Behind House

            _rooms[2, 0].Description = "This is a dimly lit forest, with large trees all around. To the east, there appears to be sunlight.";           //Dense Woods
            _rooms[2, 1].Description = "You are facing the north side of a white house. There is no door here, and all the windows are barred.";        //North of House
            _rooms[2, 2].Description = "You are in a clearing, with a forest surrounding you on the west and south.";                                   //Clearing
        }

        private static readonly Room[,] _rooms =
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
    }
}

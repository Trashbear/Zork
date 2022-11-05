using System;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        public Player Player { get; }

        public IOutputService Output { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IOutputService output)
        {
            Output = output;

            Room previousRoom = null;
            bool isRunning = true;
            while (isRunning)
            {
                Output.WriteLine(Player.CurrentRoom);
                if (previousRoom != Player.CurrentRoom)
                {
                    Output.WriteLine(Player.CurrentRoom.Description);
                    foreach (Item roomItem in Player.CurrentRoom.Inventory)
                    {
                        Output.WriteLine($"{roomItem.Description}");
                    }
                    previousRoom = Player.CurrentRoom;
                }

                Output.Write("> ");

                string inputString = Console.ReadLine().Trim();
                // might look like:  "LOOK", "TAKE MAT", "QUIT"
                char  separator = ' ';
                string[] commandTokens = inputString.Split(separator);
                
                string verb = null;
                string subject = null;
                if (commandTokens.Length == 0)
                {
                    continue;
                }
                else if (commandTokens.Length == 1)
                {
                    verb = commandTokens[0];

                }
                else
                {
                    verb = commandTokens[0];
                    subject = commandTokens[1];
                }

                Commands command = ToCommand(verb);
                string outputString;
                switch (command)
                {
                    case Commands.Quit:
                        isRunning = false;
                        outputString = "Thank you for playing!";
                        break;

                    case Commands.Look:
                        outputString = $"{Player.CurrentRoom.Description}\n";
                        foreach (Item roomItem in Player.CurrentRoom.Inventory)
                        {
                            outputString += ($"{roomItem.Description}\n");
                        }
                        break;

                    case Commands.North:
                    case Commands.South:
                    case Commands.East:
                    case Commands.West:
                        Directions direction = (Directions)command;
                        if (Player.Move(direction))
                        {
                            outputString = $"You moved {direction}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;

                    case Commands.Take:
                        if(subject != null)
                        {
                            Take(subject);
                        }
                        else
                        {
                            Output.WriteLine("What are you trying to take?");
                        }
                        outputString = null;
                        break;

                    case Commands.Drop:
                        if (subject != null)
                        {
                            Drop(subject);
                        }
                        else
                        {
                            Output.WriteLine("What item are you trying to drop?");
                        }

                        if (Player.Inventory.Count == 0)
                        {
                            Output.WriteLine("You are empty handed");
                        }
                        outputString = null;
                        break;

                    case Commands.Inventory:
                        outputString = null;
                        if (Player.Inventory.Count >= 1)
                        {
                            foreach (Item playerItem in Player.Inventory)
                            {
                                outputString += playerItem.Description;
                            }
                        }
                        else
                        {
                            outputString = "You are empty handed.";
                        }
                        break;

                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Output.WriteLine(outputString);
            }
        }

        public void Take (string itemName)
        {
            Item itemToTake = null;
            bool itemTaken = false;
            for (int i = 0; i < Player.CurrentRoom.Inventory.Count; i++)
            {
                Item item = Player.CurrentRoom.Inventory[i];
                if (string.Compare(item.Name, itemName, ignoreCase: true) ==0)
                {
                    itemTaken = true;
                    itemToTake = item;
                    Output.WriteLine("Taken");
                    Player.AddInventory(item);
                    Player.CurrentRoom.Inventory.Remove(item);                  
                }
            }
            if (itemTaken == false)
            {
                Output.WriteLine("No such item exists in the room.");
            }
        }

        public void Drop(string itemName)
        {
            Item itemToDrop = null;
            bool itemDropped = false;
            for (int i = 0; i < Player.Inventory.Count; i++)
            {
                Item item = Player.Inventory[i];
                if (string.Compare(item.Name, itemName, ignoreCase: true) == 0)
                {
                    itemDropped = true;
                    itemToDrop = item;
                    Player.Inventory.Remove(item);
                    Player.CurrentRoom.Inventory.Add(item);
                    Output.WriteLine("Dropped");
                }
            }
            if (itemDropped == false)
            {
                Output.WriteLine("No such item exists in your inventory.");
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}

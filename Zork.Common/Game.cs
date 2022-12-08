using System;
using System.Linq;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; }

        [JsonIgnore]
        public Player Player { get; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }


        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IInputService input, IOutputService output)
        {
            Input = input ?? throw new ArgumentNullException(nameof(input));
            Output = output ?? throw new ArgumentNullException(nameof(output));

            IsRunning = true;
            Input.InputReceived += OnInputReceived;
            World.RandomizeTroll();
            Output.WriteLine("Welcome to Zork!");
            Look();
            Output.WriteLine($"\n{Player.CurrentRoom}");
        }

        public void OnInputReceived(object sender, string inputString)
        {
            char separator = ' ';
            string[] commandTokens = inputString.Split(separator);

            string verb;
            string subject = null;
            if (commandTokens.Length == 0)
            {
                return;
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

            Room previousRoom = Player.CurrentRoom;
            Commands command = ToCommand(verb);
            switch (command)
            {
                case Commands.Quit:
                    IsRunning = false;
                    Output.WriteLine("Thank you for playing!");
                    break;

                case Commands.Look:
                    Look();
                    DamageCheck();
                    break;

                case Commands.North:
                case Commands.South:
                case Commands.East:
                case Commands.West:
                    Directions direction = (Directions)command;
                    Player.MovesMade++;
                    Output.WriteLine(Player.Move(direction) ? $"You moved {direction}." : "The way is shut!");
                    break;

                case Commands.Take:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Take(subject);
                    }
                    DamageCheck();
                    break;

                case Commands.Drop:
                    if (string.IsNullOrEmpty(subject))
                    {
                        Output.WriteLine("This command requires a subject.");
                    }
                    else
                    {
                        Drop(subject);
                    }
                    DamageCheck();
                    break;

                case Commands.Inventory:
                    if (Player.Inventory.Count() == 0)
                    {
                        Output.WriteLine("You are empty handed.");
                    }
                    else
                    {
                        Output.WriteLine("You are carrying:");
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine(item.InventoryDescription);
                        }
                    }
                    DamageCheck();
                    break;

                case Commands.Reward:
                    Player.Score++;
                    Output.WriteLine("Score increased!");
                    DamageCheck();
                    break;

                case Commands.Score:
                    Output.WriteLine($"Your score is {Player.Score}");
                    break;

                case Commands.Scream:
                    FindTroll();
                    DamageCheck();
                    break;

                case Commands.Health:
                    Output.WriteLine($"You gave {Player.Health} health remaining.");
                    DamageCheck();
                    break;

                case Commands.Heal:
                    Output.WriteLine("You cast a spell to heal yourself!");
                    Output.WriteLine($"You gave {Player.Health} health remaining.");
                    DamageCheck();
                    break;

                case Commands.Attack:
                    if (subject! != null)
                    {
                        bool itemCheck = false;
                        foreach (Item item in Player.Inventory)
                        {
                            if (item.Name == "Sword")
                            {
                                itemCheck = true;
                                break;
                            }
                        }
                        AttackCheck(itemCheck, subject);

                    }
                    else
                    {
                        Output.WriteLine("What are you trying to hit?");
                    }
                    DamageCheck();
                    break;

                default:
                    Output.WriteLine("Unknown command.");
                    break;
            }

            if (ReferenceEquals(previousRoom, Player.CurrentRoom) == false)
            {
                Look();
            }

            if (IsRunning == true)
            {
                Output.WriteLine($"\n{Player.CurrentRoom}");
            }
        }
        
        private void Look()
        {
            Output.WriteLine(Player.CurrentRoom.Description);
            foreach (Item item in Player.CurrentRoom.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }
            
            if(Player.CurrentRoom.Troll == true)
            {
                Output.WriteLine("");
                Output.WriteLine("A Troll prepares to strike!");
            }
            
        }

        private void Take(string itemName)
        {
            Item itemToTake = Player.CurrentRoom.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToTake == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                if(itemToTake.Reward == true)
                {
                    Player.Score++;
                    itemToTake.Reward = false;
                }
                Player.AddItemToInventory(itemToTake);
                Player.CurrentRoom.RemoveItemFromInventory(itemToTake);
                Output.WriteLine("Taken.");
            }
        }

        private void Drop(string itemName)
        {
            Item itemToDrop = Player.Inventory.FirstOrDefault(item => string.Compare(item.Name, itemName, ignoreCase: true) == 0);
            if (itemToDrop == null)
            {
                Output.WriteLine("You can't see any such thing.");                
            }
            else
            {
                Player.CurrentRoom.AddItemToInventory(itemToDrop);
                Player.RemoveItemFromInventory(itemToDrop);
                Output.WriteLine("Dropped.");
            }
        }

        private void AttackCheck (bool check, string victim)
        {
            if (check == true)
            {
                Attack(victim);
            }
            else
            {
                Output.WriteLine("You have nothing to attack with.");
            }
        }
        private void Attack(string target)
        {
            bool targetCheck = string.Equals(target, "Troll", StringComparison.OrdinalIgnoreCase);
            if (targetCheck = true && Player.CurrentRoom.Troll == true)
            {
               if(Player.CurrentRoom.TrollHealth > 1)
                {
                    Output.WriteLine("You slash at the Troll!");
                    Player.CurrentRoom.TrollHealth--;
                }
                else
                {
                   Output.WriteLine("The troll is slain! It drops a gleaming trophy.");
                    Player.CurrentRoom.AddItemToInventory(World.Items[3]);
                    Player.CurrentRoom.Troll = false;
                }
            }
            else
            {
                Output.WriteLine("There is nothing to attack");
            }
            
        }

        private void DamageCheck()
        {
            if (Player.CurrentRoom.Troll == true)
            {
                Damage();
            }
        }
        private void Damage()
        {
            if (Player.Health > 1)
            {
                Output.WriteLine("The troll swings at you!");
                Player.Health--;
                Output.WriteLine($"You gave {Player.Health} health remaining.");
            }
            else
            {
                Output.WriteLine("The Troll delivers the finishing blow. You lose.");
                IsRunning = false;
                Output.WriteLine("Thank you for playing!");
            }
        }

        private void FindTroll()
        {
            int roomIndex = World.dangerRoom;
            Room trollRoom = World.Rooms[roomIndex];
            if (World.Rooms[roomIndex].Troll == true)
            {
                Output.WriteLine($"You shout. You hear a roar from {trollRoom.Name}");
            }
            else
            {
                Output.WriteLine($"You shout. Your voice echoes through the air.");
            }
        }

        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.Unknown;
    }
}
namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string LookDescription { get; }

        public string InventoryDescription { get; }

        public bool Reward { get; set; }

        public Item(string name, string lookDescription, string inventoryDescription, bool reward)
        {
            Name = name;
            LookDescription = lookDescription;
            InventoryDescription = inventoryDescription;
            Reward = reward;
        }

        public override string ToString() => Name;
    }
}
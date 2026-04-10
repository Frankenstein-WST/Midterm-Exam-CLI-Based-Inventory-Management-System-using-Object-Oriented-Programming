namespace InventorySystem.Models
{
    public class Category
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Category(int id, string name, string description = "")
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}

using SQLite;

namespace assignment_2425.Models
{
    [Table("ShoppingItems")]
    public class ShoppingItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

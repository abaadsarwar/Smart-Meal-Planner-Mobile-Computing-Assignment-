using SQLite;

namespace assignment_2425.Models;

[Table("Meals")]
public class Meal
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }
}

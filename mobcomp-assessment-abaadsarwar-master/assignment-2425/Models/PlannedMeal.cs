using SQLite;

namespace assignment_2425.Models
{
    [Table("PlannedMeals")]
    public class PlannedMeal
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Day { get; set; } // e.g. "Monday"
        public string TimeOfDay { get; set; } // e.g. "Lunch"
        public string MealName { get; set; } // e.g. "Chicken Wrap"
    }
}

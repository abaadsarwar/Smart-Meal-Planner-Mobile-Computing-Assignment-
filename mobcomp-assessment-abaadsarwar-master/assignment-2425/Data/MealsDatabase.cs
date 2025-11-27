using assignment_2425.Models;
using SQLite;

namespace assignment_2425.Data
{
    public class MealsDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public MealsDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Meal>().Wait();
            _database.CreateTableAsync<PlannedMeal>().Wait();
            _database.CreateTableAsync<ShoppingItem>().Wait();
        }

        // Meals
        public Task<List<Meal>> GetMealsAsync() => _database.Table<Meal>().ToListAsync();
        public Task<int> SaveMealAsync(Meal meal) =>
            meal.Id == 0 ? _database.InsertAsync(meal) : _database.UpdateAsync(meal);
        public Task<int> DeleteMealAsync(Meal meal) =>
            _database.DeleteAsync(meal);

        // Planned Meals
        public Task<List<PlannedMeal>> GetPlannedMealsAsync() => _database.Table<PlannedMeal>().ToListAsync();
        public Task<int> SavePlannedMealAsync(PlannedMeal plannedMeal) =>
            _database.InsertAsync(plannedMeal);
        public Task<int> DeletePlannedMealAsync(PlannedMeal plannedMeal) =>
            _database.DeleteAsync(plannedMeal);



        // Shopping Items
        public Task<List<ShoppingItem>> GetShoppingItemsAsync() => _database.Table<ShoppingItem>().ToListAsync();
        public Task<int> SaveShoppingItemAsync(ShoppingItem item) =>
            item.Id == 0 ? _database.InsertAsync(item) : _database.UpdateAsync(item);
        public Task<int> DeleteShoppingItemAsync(ShoppingItem item) => _database.DeleteAsync(item);

    }
}

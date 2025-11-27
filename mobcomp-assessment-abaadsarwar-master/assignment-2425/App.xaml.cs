using assignment_2425.Data; // ✅ Make sure this is added
using System.IO;

namespace assignment_2425;

public partial class App : Application
{
    private static MealsDatabase _database;

    public static MealsDatabase Database
    {
        get
        {
            if (_database == null)
            {
                string dbPath = Path.Combine(FileSystem.AppDataDirectory, "meals.db3");
                _database = new MealsDatabase(dbPath);
            }
            return _database;
        }
    }

    public App()
    {
        InitializeComponent();
        MainPage = new AppShell(); // ✅ Make sure AppShell exists
    }
}

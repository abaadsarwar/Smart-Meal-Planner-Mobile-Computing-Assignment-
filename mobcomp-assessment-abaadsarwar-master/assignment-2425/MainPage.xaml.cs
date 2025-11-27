using assignment_2425.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.Linq;

namespace assignment_2425;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Clear any previous meal summaries
        MealSummaryLayout.Children.Clear();

        // Get current day
        var today = DateTime.Now.DayOfWeek.ToString();

        // Load data from local database
        var plannedMeals = await App.Database.GetPlannedMealsAsync();
        var allMeals = await App.Database.GetMealsAsync();


        var todayMeals = plannedMeals.Where(pm => pm.Day == today).ToList();

        // If no meals are planned, show a message
        if (!todayMeals.Any())
        {
            MealSummaryLayout.Children.Add(new Label
            {
                Text = "No meals planned yet. Go to the planner to add some!",
                TextColor = Colors.Gray,
                FontAttributes = FontAttributes.Italic,
                HorizontalTextAlignment = TextAlignment.Center
            });
        }
        else
        {
            // For each meal time, display meal info
            foreach (string time in new[] { "Breakfast", "Lunch", "Dinner" })
            {
                var planned = todayMeals.FirstOrDefault(m => m.TimeOfDay == time);
                if (planned != null)
                {
                    var meal = allMeals.FirstOrDefault(m => m.Name == planned.MealName);
                    MealSummaryLayout.Children.Add(new Label
                    {
                        Text = $"{time}: {meal?.Name ?? "Not planned"}\n{meal?.Description ?? ""}"
                    });
                }
                else
                {
                    MealSummaryLayout.Children.Add(new Label
                    {
                        Text = $"{time}: Not planned"
                    });
                }
            }
        }
    }

    // Called when the speech button is clicked
    private async void OnReadSummaryClicked(object sender, EventArgs e)
    {
        var today = DateTime.Now.DayOfWeek.ToString();
        var plannedMeals = await App.Database.GetPlannedMealsAsync();
        var allMeals = await App.Database.GetMealsAsync();
        var todayMeals = plannedMeals.Where(pm => pm.Day == today).ToList();

        // If no meals are planned, show an alert
        if (!todayMeals.Any())
        {
            await DisplayAlert("No Meals Planned", "You haven't planned any meals for today. Go to the planner to add some!", "OK");
            return;
        }

        // Build speech summary
        string summary = $"Here's your meal plan for {today}. ";

        foreach (string time in new[] { "Breakfast", "Lunch", "Dinner" })
        {
            var planned = todayMeals.FirstOrDefault(m => m.TimeOfDay == time);
            if (planned != null)
            {
                var meal = allMeals.FirstOrDefault(m => m.Name == planned.MealName);
                summary += $"{time}: {meal?.Name ?? "Not planned"}. {meal?.Description ?? ""}. ";
            }
            else
            {
                summary += $"{time}: Not planned. ";
            }
        }

        // Text to speech for the summary
        await TextToSpeech.Default.SpeakAsync(summary);
    }

    // Navigate to the Meal Planner page
    private async void OnMealPlannerClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MealPlannerPage());
    }

    // Navigate to the Meals Library page
    private async void OnMealsLibraryClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MealsLibraryPage());
    }

    // Navigate to the Shopping List page
    private async void OnShoppingListClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShoppingListPage());
    }

    // Navigate to the Settings page
    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
}

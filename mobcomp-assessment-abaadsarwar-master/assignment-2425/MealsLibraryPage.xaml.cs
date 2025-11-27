using System.Collections.ObjectModel;
using assignment_2425.Models;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Controls;

namespace assignment_2425;

public partial class MealsLibraryPage : ContentPage
{
    public ObservableCollection<Meal> Meals { get; set; } = new();

    public MealsLibraryPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    // Load meals when page appears
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            Meals.Clear();
            var mealsFromDb = await App.Database.GetMealsAsync();
            foreach (var meal in mealsFromDb)
                Meals.Add(meal);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load meals: {ex.Message}", "OK");
        }
    }

    // Handle Add button click
    private async void OnAddMealClicked(object sender, EventArgs e)
    {
        try
        {
            string name = await DisplayPromptAsync("New Meal", "Enter meal name:");
            if (string.IsNullOrWhiteSpace(name))
            {
                await DisplayAlert("Validation", "Meal name cannot be empty.", "OK");
                return;
            }

            string description = await DisplayPromptAsync("New Meal", "Enter description (e.g. calories, nutrition info):");
            if (string.IsNullOrWhiteSpace(description))
            {
                await DisplayAlert("Validation", "Description cannot be empty.", "OK");
                return;
            }

            var newMeal = new Meal { Name = name.Trim(), Description = description.Trim() };
            Meals.Add(newMeal);
            await App.Database.SaveMealAsync(newMeal);
            TryVibrate();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add meal: {ex.Message}", "OK");
        }
    }

    // Handle Edit button click
    private async void OnEditMealClicked(object sender, EventArgs e)
    {
        var meal = (sender as Button)?.BindingContext as Meal;
        if (meal == null) return;

        try
        {
            string newName = await DisplayPromptAsync("Edit Meal", "Update meal name:", initialValue: meal.Name);
            if (string.IsNullOrWhiteSpace(newName))
            {
                await DisplayAlert("Validation", "Meal name cannot be empty.", "OK");
                return;
            }

            string newDesc = await DisplayPromptAsync("Edit Meal", "Update description:", initialValue: meal.Description);
            if (string.IsNullOrWhiteSpace(newDesc))
            {
                await DisplayAlert("Validation", "Description cannot be empty.", "OK");
                return;
            }

            meal.Name = newName.Trim();
            meal.Description = newDesc.Trim();
            await App.Database.SaveMealAsync(meal);

            // Refresh UI
            MealsList.ItemsSource = null;
            MealsList.ItemsSource = Meals;

            TryVibrate();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to update meal: {ex.Message}", "OK");
        }
    }

    // Handle Delete button click
    private async void OnDeleteMealClicked(object sender, EventArgs e)
    {
        var meal = (sender as Button)?.BindingContext as Meal;
        if (meal == null) return;

        bool confirm = await DisplayAlert("Delete Meal", $"Are you sure you want to delete '{meal.Name}'?", "Yes", "Cancel");
        if (!confirm) return;

        try
        {
            Meals.Remove(meal);
            await App.Database.DeleteMealAsync(meal);
            TryVibrate();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to delete meal: {ex.Message}", "OK");
        }
    }

    // Attempt vibration with error handling
    private void TryVibrate()
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
        }
        catch (FeatureNotSupportedException)
        {
            // Device does not support vibration
        }
    }
}

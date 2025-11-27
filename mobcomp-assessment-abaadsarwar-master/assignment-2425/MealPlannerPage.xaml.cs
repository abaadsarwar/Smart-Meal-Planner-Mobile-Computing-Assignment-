using assignment_2425.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;

namespace assignment_2425;

public partial class MealPlannerPage : ContentPage
{
    private ObservableCollection<Meal> meals;
    private readonly Dictionary<string, Picker> pickerMap = new();
    private readonly Dictionary<string, string> previousPlan = new();

    private readonly string today = DateTime.Today.DayOfWeek.ToString();
    private readonly string[] mealTimes = ["Breakfast", "Lunch", "Dinner"];

    private DateTime lastShakeTime = DateTime.MinValue;
    private bool isAlertDisplayed = false;

    public MealPlannerPage()
    {
        InitializeComponent();
        Accelerometer.ShakeDetected += OnShakeDetected;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Accelerometer.Start(SensorSpeed.Game); // Start shake detection
        await LoadMealsAndBuildPlanner();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Accelerometer.Stop(); // Stop accelerometer when leaving page
    }

 // Load meals from database and construct the daily planner

    private async Task LoadMealsAndBuildPlanner()
    {
        PlannerLayout.Children.Clear();
        pickerMap.Clear();

        try
        {
            meals = new ObservableCollection<Meal>(await App.Database.GetMealsAsync());
            var plannedToday = await App.Database.GetPlannedMealsAsync();
            var todayMeals = plannedToday
                .Where(p => p.Day == today)
                .ToDictionary(p => p.TimeOfDay, p => p.MealName);

            if (meals.Count == 0)
            {
                PlannerLayout.Children.Add(new Label
                {
                    Text = "No meals saved yet. Please add meals in the Meals Library.",
                    FontAttributes = FontAttributes.Italic,
                    TextColor = Colors.Gray,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Margin = new Thickness(0, 20)
                });
                return;
            }

            PlannerLayout.Children.Add(new Label
            {
                Text = $"📆 {today}",
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                Margin = new Thickness(0, 10, 0, 5)
            });

            PlannerLayout.Children.Add(new Label
            {
                Text = "💡 Tip: Shake your device to randomise meals!",
                FontSize = 14,
                TextColor = Application.Current.RequestedTheme == AppTheme.Dark
                    ? Colors.White
                    : Colors.DarkSlateGray,
                Margin = new Thickness(0, 0, 0, 10)
            });

            // Add pickers for each meal time
            foreach (var time in mealTimes)
            {
                var picker = new Picker
                {
                    Title = $"Select {time}",
                    ItemsSource = meals.Select(m => m.Name).ToList(),
                    WidthRequest = 300,
                    FontSize = 14,
                    FontFamily = "OpenSansRegular",
                    TextColor = Application.Current.RequestedTheme == AppTheme.Dark ? Colors.White : Colors.Black,
                    TitleColor = Application.Current.RequestedTheme == AppTheme.Dark ? Colors.LightGray : Colors.DarkSlateGray,
                    BackgroundColor = Application.Current.RequestedTheme == AppTheme.Dark ? Colors.DarkSlateGray : Colors.WhiteSmoke
                };

                if (todayMeals.TryGetValue(time, out string selected))
                    picker.SelectedItem = selected;

                pickerMap[time] = picker;

                PlannerLayout.Children.Add(new HorizontalStackLayout
                {
                    Spacing = 10,
                    Children =
                    {
                        new Label
                        {
                            Text = $"{time}:",
                            WidthRequest = 90,
                            VerticalTextAlignment = TextAlignment.Center
                        },
                        picker
                    }
                });
            }

            // Save button
            var saveButton = new Button
            {
                Text = "Save Meal Plan",
                BackgroundColor = Colors.Green,
                TextColor = Colors.White,
                CornerRadius = 10,
                Margin = new Thickness(0, 20, 0, 10)
            };
            saveButton.Clicked += OnSavePlannerClicked;

            // Reset button
            var resetButton = new Button
            {
                Text = "Reset Meal Plan",
                BackgroundColor = Colors.Red,
                TextColor = Colors.White,
                CornerRadius = 10,
                Margin = new Thickness(0, 0, 0, 40)
            };
            resetButton.Clicked += OnResetPlannerClicked;

            PlannerLayout.Children.Add(saveButton);
            PlannerLayout.Children.Add(resetButton);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to load planner: {ex.Message}", "OK");
        }
    }

    // Save selected meals to the database

    private async void OnSavePlannerClicked(object sender, EventArgs e)
    {
        try
        {
            bool hasSelection = pickerMap.Values.Any(picker => !string.IsNullOrWhiteSpace(picker.SelectedItem?.ToString()));

            if (!hasSelection)
            {
                await DisplayAlert("Validation", "You must select at least one meal before saving the plan.", "OK");
                return;
            }

            var currentPlans = await App.Database.GetPlannedMealsAsync();
            foreach (var plan in currentPlans.Where(p => p.Day == today))
                await App.Database.DeletePlannedMealAsync(plan);

            foreach (var (time, picker) in pickerMap)
            {
                var mealName = picker.SelectedItem?.ToString();
                if (!string.IsNullOrWhiteSpace(mealName))
                {
                    var plannedMeal = new PlannedMeal
                    {
                        Day = today,
                        TimeOfDay = time,
                        MealName = mealName
                    };
                    await App.Database.SavePlannedMealAsync(plannedMeal);
                }
            }

            await DisplayAlert("Saved", "Today's meal plan saved successfully.", "OK");
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(300));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save plan: {ex.Message}", "OK");
        }
    }



    // reset plan with confirmation

    private async void OnResetPlannerClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Reset", "Are you sure you want to clear today's plan?", "Yes", "Cancel");
        if (!confirm) return;

        try
        {
            var allPlans = await App.Database.GetPlannedMealsAsync();
            foreach (var pm in allPlans.Where(p => p.Day == today))
                await App.Database.DeletePlannedMealAsync(pm);

            await LoadMealsAndBuildPlanner();
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(250));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not reset planner: {ex.Message}", "OK");
        }
    }


    //  shake to randomise meal selection

    private async void OnShakeDetected(object sender, EventArgs e)
    {
        if (meals == null || meals.Count == 0 || (DateTime.Now - lastShakeTime).TotalSeconds < 1.5 || isAlertDisplayed)
            return;

        lastShakeTime = DateTime.Now;
        isAlertDisplayed = true;

        try
        {
            foreach (var time in mealTimes)
            {
                if (pickerMap.TryGetValue(time, out var picker))
                    previousPlan[time] = picker.SelectedItem?.ToString() ?? "";
            }

            var random = new Random();
            foreach (var time in mealTimes)
            {
                if (pickerMap.TryGetValue(time, out var picker))
                {
                    var randomMeal = meals[random.Next(meals.Count)];
                    picker.SelectedItem = randomMeal.Name;
                }
            }

            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(250));

            bool undo = await MainThread.InvokeOnMainThreadAsync(() =>
                DisplayAlert("Shuffled!", "Random meals selected for today. Undo?", "Undo", "Keep"));

            if (undo)
            {
                foreach (var time in mealTimes)
                {
                    if (pickerMap.TryGetValue(time, out var picker) && previousPlan.ContainsKey(time))
                        picker.SelectedItem = previousPlan[time];
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Shake failed: {ex.Message}", "OK");
        }

        isAlertDisplayed = false;
    }
}

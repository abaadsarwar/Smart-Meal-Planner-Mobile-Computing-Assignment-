using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices;

namespace assignment_2425;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Set the switch toggle state based on the current app theme
        DarkModeSwitch.IsToggled = Application.Current.UserAppTheme == AppTheme.Dark;
    }

    private void OnDarkModeToggled(object sender, ToggledEventArgs e)
    {
        // Change the app theme
        Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;


        string theme = e.Value ? "Dark" : "Light";
        Shell.Current.DisplayAlert("Theme Changed", $"Theme set to {theme} Mode", "OK");

        // Provide haptic feedback using vibration
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(150));
        }
        catch (FeatureNotSupportedException)
        {
            // Device doesn't support vibration – silently ignore
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Vibration failed: {ex.Message}");
        }
    }
}

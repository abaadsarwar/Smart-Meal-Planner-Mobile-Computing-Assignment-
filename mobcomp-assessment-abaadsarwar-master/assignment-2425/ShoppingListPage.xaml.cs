using assignment_2425.Models;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Networking;
using System.Collections.ObjectModel;

namespace assignment_2425;

public partial class ShoppingListPage : ContentPage
{
    public ObservableCollection<ShoppingItem> Items { get; set; } = new();

    public ShoppingListPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    // Load data when the page appears
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            var items = await App.Database.GetShoppingItemsAsync();
            Items.Clear();
            foreach (var item in items)
                Items.Add(item);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Unable to load shopping items: {ex.Message}", "OK");
        }
    }

    // Add a new item to the list
    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        string itemName = ItemEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(itemName))
        {
            await DisplayAlert("Validation", "Please enter a valid item name.", "OK");
            return;
        }

        try
        {
            var item = new ShoppingItem { Name = itemName };
            await App.Database.SaveShoppingItemAsync(item);
            Items.Add(item);
            ItemEntry.Text = string.Empty;
            TryVibrate();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to add item: {ex.Message}", "OK");
        }
    }

    // Delete an item from the list
    private async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        var item = (sender as Button)?.BindingContext as ShoppingItem;
        if (item == null) return;

        bool confirm = await DisplayAlert("Delete Item", $"Are you sure you want to delete '{item.Name}'?", "Yes", "Cancel");
        if (!confirm) return;

        try
        {
            Items.Remove(item);
            await App.Database.DeleteShoppingItemAsync(item);
            TryVibrate();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to delete item: {ex.Message}", "OK");
        }
    }

    // Edit an item
    private async void OnEditItemClicked(object sender, EventArgs e)
    {
        var item = (sender as Button)?.BindingContext as ShoppingItem;
        if (item == null) return;

        string updated = await DisplayPromptAsync("Edit Item", "Update the item name:", initialValue: item.Name);
        if (string.IsNullOrWhiteSpace(updated))
        {
            await DisplayAlert("Validation", "Item name cannot be empty.", "OK");
            return;
        }

        try
        {
            item.Name = updated.Trim();
            await App.Database.SaveShoppingItemAsync(item);

            // Refresh UI manually
            int index = Items.IndexOf(item);
            Items.Remove(item);
            Items.Insert(index, item);

            TryVibrate();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to update item: {ex.Message}", "OK");
        }
    }

    // Open Google Maps to find nearby shops using hte users location
    private async void OnFindShopClicked(object sender, EventArgs e)
    {
        try
        {
            // Check for internet access
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("No Internet", "Please connect to Wi-Fi or mobile data to find nearby shops.", "OK");
                return;
            }

            // Get device location
            var location = await Geolocation.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync();

            if (location != null)
            {
                var query = Uri.EscapeDataString("grocery store");
                var uri = $"https://www.google.com/maps/search/{query}/@{location.Latitude},{location.Longitude},15z";
                await Launcher.Default.OpenAsync(uri);
            }
            else
            {
                await DisplayAlert("Location Error", "Unable to determine your location.", "OK");
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "Location services are not supported on this device.", "OK");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Error", "Location permission not granted. Please check your app settings.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Unexpected Error", $"Something went wrong: {ex.Message}", "OK");
        }
    }

    // Vibration
    private void TryVibrate()
    {
        try
        {
            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Vibration failed: {ex.Message}");
        }
    }
}

# Smart-Meal-Planner-Mobile-Computing-Assignment-
A cross-platform meal planning and food management app built using .NET MAUI for the Mobile Computing university module. The app helps users plan meals, store recipes, generate shopping lists, and explore food options through an intuitive, modern interface.


⭐ Features
Meal Storage (SQLite Local DB): Add, edit, delete, and view meals locally on device.
Daily Meal Planner: Plan breakfast, lunch, dinner, and snacks for a single day with persistent storage.
Shake-to-Select Meal: Uses the accelerometer to randomly choose a meal when the device is shaken.
Shopping List: Add custom items, mark as complete, edit, delete, and save in local storage.
Nearest Shop Finder: Uses device location services to help users find nearby stores.
Theming: Fully implemented light/dark mode toggle with consistent colors based on WCAG guidelines.
Responsive UI: All pages optimised for both mobile and tablet layouts.
Error Handling & Validation: Every page includes robust validation, safe database operations, and user-friendly feedback.


🧱 Technologies Used
.NET MAUI (C#)
SQLite local database
MVVM architecture
XAML for UI
Location APIs
Accelerometer / Sensors API


🎯 Purpose of the Project
This app was built as part of a university assessment to demonstrate:
Cross-platform development using .NET MAUI
Use of device features (sensors, location)
Local database integration
UI/UX principles, accessibility, and responsiveness
Clean code with comments, validation, and testing


📁 Structure
/Models – Data models (Meal, PlannedMeal, ShoppingItem)
/Views – XAML pages
/ViewModels – Logic for each page (MVVM)
/Services – SQLite database and helper services
/Resources – Colors, styles, images, and themes


🚀 How to Run
Clone the repo
Open in Visual Studio 2022
Select Android Emulator or physical device
Run the project

# Smart-Meal-Planner-Mobile-Computing-Assignment-
Smart Meal Planner is a cross-platform food and meal management app built using .NET MAUI for the Mobile Computing (6G6Z0014) module.
The app allows users to plan daily meals, maintain a meal library, manage a shopping list, and use device features such as shake detection, location services, haptic feedback, and text-to-speech. It is fully responsive across phones and tablets with WCAG-compliant light and dark themes.



⭐ Features
📌 Meal Planner
Plan Breakfast, Lunch, and Dinner
Shake-to-select meals using the accelerometer
Save or reset today’s plan (stored locally via SQLite)
Validation prevents empty or invalid plans
Includes haptic feedback on all actions

📌 Meals Library
Add, edit, and delete meals
SQLite-powered data persistence
Validation + error handling
Responsive layouts for tablets

📌 Shopping List
Add, edit, delete shopping items
Vibration feedback for each action
Find nearest grocery store using:
Device GPS
Connectivity check
Opens Google Maps automatically

📌 Home Page
Shows today’s planned meals
Text-to-Speech (TTS) reads out the meal plan
If no meals are planned, TTS is skipped and a message is shown

📌 Settings
Toggle Light/Dark mode
Uses WCAG-compliant colors
Includes haptic feedback for accessibility



🔧 Device Features Used
Feature	Purpose	Used In
Haptic Feedback (Vibration)	Confirms actions	Meal Planner, Meals Library, Shopping List, Settings
Accelerometer (Shake Detection)	Random meal generation	Meal Planner
Location Services	Find nearest shop	Shopping List
Text-to-Speech (TTS)	Read today's meals out loud	Home Page



🧱 Technologies Used
.NET MAUI (C#)
SQLite local database
MVVM architecture
XAML UI
Location API & Sensors API
Android deployment & testing




📁 Project Structure
/Models       → Meal, PlannedMeal, ShoppingItem
/Views        → UI pages (XAML)
/ViewModels   → Logic for each page (MVVM)
/Services     → SQLite DB and helpers
/Resources    → Styles, colours, images, themes




🎯 Purpose of the Project
This project demonstrates:
Cross-platform mobile development with .NET MAUI
Integration of device sensors (Accelerometer, GPS)
Local database storage for offline use
Accessible UI/UX with WCAG compliance
Responsive layouts for tablets and phones
Validated, error-free user interactions with feedback



🧪 Testing & Deployment
Tested on:
Android physical device
Android tablet emulator
Validated across portrait/landscape
Ensured stability for:
Shake detection
Database operations
Location permissions
Offline/online transitions




🚀 How to Run
Clone the repository
Open the solution in Visual Studio 2022
Select an Android emulator or physical device
Press Run

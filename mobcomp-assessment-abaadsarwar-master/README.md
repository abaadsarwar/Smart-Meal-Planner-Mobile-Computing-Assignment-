[![Review Assignment Due Date](https://classroom.github.com/assets/deadline-readme-button-22041afd0340ce965d47ae6ef1cefeee28c7c493a6346c4f15d667ab976d596c.svg)](https://classroom.github.com/a/rNdN2Yn1)
# Mobile Computing Assessment 24/25

Smart Meal Planner is an app which helps users plan their daily meals, manage a meal library, maintain a shopping list, and explore meal ideas. The app is designed for both phones and tablets, featuring responsive layouts and WCAG-compliant colors with full support for light and dark modes. Functions I have used include: 
Haptic Feedback (Vibration)
Purpose: Provides tactile feedback to the user to confirm an action (e.g., saving a meal, deleting an item, toggling dark mode).
Where it's used: Meal Planner (Save/Reset), Meals Library (Add/Edit/Delete), Shopping List (Add/Edit/Delete), Settings (Dark Mode toggle).

Shake Detection (Accelerometer)
Purpose: Allows users to shake their device to randomly generate a meal plan for the day.
Where it's used: Meal Planner page. The app listens for a shake gesture and assigns random meals to each meal slot (Breakfast, Lunch, Dinner). Option to undo is also provided.

Location Services
Purpose: Uses the device's GPS to detect current location and opens Google Maps to find the nearest grocery store.
Where it's used: Shopping List page. Before attempting to open Maps, it validates internet connectivity and location permission.

Text-to-Speech (TTS)
Purpose: Reads out the user’s planned meals aloud using the device’s built-in text-to-speech engine.
Where it's used: Home Page. If no meals are planned, TTS is skipped and a message is shown instead to the user.


Pages:

Meal Planner
Users can select meals for Breakfast, Lunch, and Dinner.
Shake detection (using the device accelerometer) randomly assigns meals.
Meals are persisted and saved locally using SQLite.
Option to reset or save today’s plan.
Validation prevents saving an empty plan.
Fully styled for dark/light mode and tablet responsiveness.

 Meals Library
Add, edit, or delete custom meals with a name and description.
Data is stored locally using SQLite.
Interface includes validation, error handling, and vibration feedback.
Responsive layout for tablet support.

Shopping List
Add, edit, or remove items in a custom shopping list.
Each item includes vibrational feedback.
Find nearby grocery stores using the device's geolocation and maps.
Includes connectivity check before launching maps.

Settings
Toggle between light and dark mode.
Saves preference for the session.
Haptic feedback on toggle for better accessibility.

Home Page
Displays a summary of today’s planned meals.
Text-to-Speech reads the meal summary aloud.
If no meals are planned, a relevant message is shown and TTS is skipped.
Quick navigation to all major pages.

Accessibility and Usability
All color schemes comply with WCAG contrast guidelines.
Responsive UI for both phones and tablets.
Includes vibration feedback on user interaction.
Error handling and validation across all pages to improve reliability.

Testing & Deployment
Tested on Android physical device and tablet emulator.
Fully functional across different screen sizes and orientations.
Ensures robust behavior with shake gestures, database interactions, and offline handling.

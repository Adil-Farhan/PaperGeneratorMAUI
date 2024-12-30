# Question Paper Maker

A .NET MAUI application for creating and managing question papers.

## Prerequisites

- Visual Studio 2022 17.3 or later with the .NET MAUI workload installed
- .NET 7.0 SDK or later
- For Android development: Android SDK and emulator
- For iOS development: Mac with Xcode 13 or later

## Setup

1. Clone the repository
```bash
git clone <repository-url>
cd QuestionPaperMaker
```

2. Start the backend server (FastAPI)
```bash
cd backend
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
pip install -r requirements.txt
uvicorn app.main:app --reload
```

3. Open the solution in Visual Studio
- Open `MAUI/QuestionPaperMaker/QuestionPaperMaker.sln`
- Restore NuGet packages
- Build the solution

4. Configure the API endpoint
- Update `AppConstants.cs` with your backend server URL
  - For Android Emulator: `http://10.0.2.2:8000`
  - For Windows/iOS: `http://localhost:8000`

5. Run the application
- Select your target platform (Windows/Android/iOS/macOS)
- Press F5 or click Run

## Project Structure

```
QuestionPaperMaker/
├── Models/             # Data models
├── ViewModels/         # MVVM ViewModels
├── Views/              # XAML UI pages
├── Services/           # Business logic and API communication
├── Constants/          # Application constants
└── Resources/          # Styles and assets
```

## Missing Components

To complete the application, we still need to:

1. Add the following files:
   - App.xaml and App.xaml.cs (main application entry point)
   - AppShell.xaml and AppShell.xaml.cs (navigation shell)
   - SettingsPage.xaml and SettingsPage.xaml.cs
   - Resource dictionaries for styles and colors

2. Register services in MauiProgram.cs:
   - ISettingsService
   - Add navigation routes

3. Add platform-specific configurations:
   - Android Manifest permissions
   - iOS Info.plist settings
   - Windows package manifest

## Development Notes

- The app uses MVVM pattern with data binding
- Settings are stored using MAUI Preferences
- API communication uses HttpClient
- PDF generation uses MAUI Graphics 
# Facebook Sample using Facebook C# SDK with Windows 8 Metro Style Application

This sample demonstrates the use of Facebook C# SDK v6 as a Windows 8 Metro Style Application.

_Note: This sample does not necessarily demonstrate the best use but rather features of using Facebook C# SDK on a Windows 8 Metro Style app. XTaskAsync methods are preferred over XAsync methods. Always remember to handle exceptions_

# Getting started

Set the appropriate `AppId` in `FacebookLoginPage.xaml.cs` before running the sample.

```csharp
private const string AppId = "app_id";
```


_**Note:**
For new projects using Facebook C# SDK make sure to enable `Internet (Client)` capability in `Package.appxmanifest` file.
This is already enabled by default when creating a new project in Visual Studio._

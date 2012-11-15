# Facebook Sample using Facebook C# SDK with Windows 8 Store Application

This sample demonstrates the use of Facebook C# SDK v6 as a Windows 8 Store Application.

_Note: This sample does not necessarily demonstrate the best use but rather features of using Facebook C# SDK on a Windows 8 Store App. XTaskAsync methods are preferred over XAsync methods. Always remember to handle exceptions_

# Getting started

Set your own Facebook App ID to the variable `_facebookAppId` on line 27 in `Views\HomePage.xaml.cs` before running the sample.

```csharp
string _facebookAppId = "app_id";
```


_**Note:**
For new projects using Facebook C# SDK make sure to enable `Internet (Client)` capability in `Package.appxmanifest` file.
This is already enabled by default when creating a new project in Visual Studio._

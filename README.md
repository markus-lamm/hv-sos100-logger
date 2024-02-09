# Guide for the use of Hv.Sos100.Logger2

## Installation

In the top menu of Visual Studio select **Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution**.

Search for **Hv.Sos100.Logger2** and install the latest version of the NuGet package.

## Using the logger

The logger is most appropriate to use when you believe that an api call may cause an error.

> [!IMPORTANT]
> Using api logging will place the log data in the log database of Group 6. Using local logging the log file will be avaliable at `C:\Temp\Hv.Sos100.Logger2.LocalLogs\Log.txt`.
> It is recommended to use api logging first and local logging as a fallback should api logging fail.


1. Create an instance of the LogService

```csharp
var logger = new Hv.Sos100.Logger2.LogService();
```

2. Call on api logging using the LogService object and collect the result in a variable

```csharp
var logResult = await logger.CreateApiLog("mySystem", "this is a message");
```

3. If the api logging fails call the local logging

```csharp
logger.CreateLocalLog("mySystem", "this is a message");
```
> [!NOTE]
> Both `CreateApiLog` and `CreateLocalLog` takes two parameters `sourceSystem` and `message`. Use your system name as the input for `sourceSystem` ex. `UserService` and describe the error in `message`.

## Example

```csharp
// Simulating a failed api call
var apiCall = false;

if (!apiCall)
{
    // Create an instance of the LogService
    var logger = new Hv.Sos100.Logger2.LogService();

    // Call the api to log the issue
    var logResult = await logger.CreateApiLog("mySystem", "this is a message");

    // If the logging api call fails use local logging
    if (!logResult)
    {
        logger.CreateLocalLog("mySystem", "this is a message");
    }
}
```

# Guide for the use of Hv.Sos100.Logger

## Installation

In the top menu of Visual Studio select **Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution**.

Search for **Hv.Sos100.Logger** and install the latest version of the NuGet package.

## Using the logger

The logger is most appropriate to use when you believe that an api call may cause an error. But can be used wherever an error may occour.

> [!IMPORTANT]
> Using api logging will place the log data in the log database of Group 6. Using local logging the log file will be avaliable in the computer running the system at `C:\Temp\Hv.Sos100.Logger.LocalLogs\Log.txt`.
> It is recommended to use api logging first and local logging as a fallback should api logging fail.

1. Add the using statement
 
```csharp
using Hv.Sos100.Logger;
```

2. Create an instance of the LogService

```csharp
var logger = new LogService();
```

3. Call on api logging using the LogService object and collect the result in a variable

```csharp
var logResult = await logger.CreateApiLog("mySystem", "this is a message");
```

4. If the api logging fails call the local logging

```csharp
logger.CreateLocalLog("mySystem", "this is a message");
```
> [!NOTE]
> Both `CreateApiLog` and `CreateLocalLog` takes two parameters `sourceSystem` and `message`. Use your system name as the input for `sourceSystem` ex. `UserService` and describe the error in `message`. You can either use the exception source and message if they are available or write custom strings.

## Example

```csharp
// Simulating a failed operation
try
{
    //Api call or other non-guaranteed operation fails
}
catch (Exception ex)
{
    // Create an instance of the LogService
    var logger = new LogService();

    // Call the api to log the issue
    var logResult = await logger.CreateApiLog(ex.Source, ex.Message);

    // If the logging api call fails use local logging
    if (!logResult)
    {
        logger.CreateLocalLog(ex.Source, ex.Message);
    }
}
```

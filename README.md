# Guide for the use of Hv.Sos100.Logger

## Installation

In the top menu of Visual Studio select **Tools -> NuGet Package Manager -> Manage NuGet Packages for Solution**.

Search for **Hv.Sos100.Logger** and install the latest version of the NuGet package.

## Using the logger

The logger should be used when any important information that should be recorded is created.

> [!IMPORTANT]
> If the log is created with the api it will be placed in the database of Group 6. If the log is created locally the log file will be avaliable in the computer running the system at `C:\Temp\Hv.Sos100.Logger.LocalLogs\Log.txt`.

1. Add the using statement
 
```csharp
using Hv.Sos100.Logger;
```

2. Create an instance of the LogService

```csharp
var logger = new LogService();
```

3. Call on the logging method using the LogService object. The first method can be used anywhere. The second method can only be used where an exception object is created, such as inside a try catch block.

```csharp
await logger.CreateLog("mySystem", Severity.Error, "this is a message");
//OR
await logger.CreateLog("mySystem", exception);
```

4. If you wish to specify where the logging should occour use the LogType parameter. The LogType parameter is optional and the method will default to Both if unspecified. Meaning it will attempt to create an api log and only a local log if unsuccessful.

 ```csharp
await logger.CreateLog("mySystem", Severity.Error, "this is a message", LogType.Api);
```

> [!NOTE]
> The NuGet consists of two public log methods both named `CreateLog`. Invoke the right method based on the parameters you use to call the method. One method expects an exception object, only use this variant inside a try catch block. The other method can be used universally and also inside a try catch block if you only want to use one variant of the method globally.

## Example

```csharp
// Simulating a failed operation inside a try catch block
try
{
    //Api call or other non-guaranteed operation fails
}
catch (Exception ex)
{
    // Create an instance of the LogService
    var logger = new LogService();

    // Call the api to log the issue
    await logger.CreateLog("mySystem", exception);
}

// Logging outside a try catch block
if(true)
{
    await logger.CreateLog("mySystem", Severity.Info, "this is a message");
}
```

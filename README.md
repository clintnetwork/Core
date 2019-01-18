# TypeDB Core
It's a serialized, in-memory and strongly typed database system developed in C#

TypeDB use the .NET Core style programming to build a standalone or client-server in-memory database.

Usage
-----
```csharp
var instance = new TypeDB.Core(TypeDB.Mode.Remote)
    .Configure(new TypeDB.Configuration
    {
        Host = "localhost",
        Port = 777,
        IsPersistent = true
    })
    .Connect();

var testDatabase = instance.OpenDatabase("test");

testDatabase.Set<int>("NumberOfCars", 28);
testDatabase.Increment<int>("NumberOfCars", 2);
var numberOfCars = testDatabase.Get<int>("NumberOfCars");
// numberOfCars equal to 30

testDatabase.Flush();
```

### Disclaimer:
We are currently in development, a production version will be released in few weeks.

## Standalone or Remote Mode
Two operating modes are available on TypeDB, the `Standalone` mode that does not require the use of a daemon, which is very convenient for embedded projects, and the `Remote` mode that is coupled with a TypeDB-Server.

## Triggers Notion
You can easily set triggers to your TypeDB database, when you make the operation of your choice and run an associated callback.

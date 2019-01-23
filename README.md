# Type-DB.Core
> **Warning**: Type-DB is currently in construction, if you would like to help us, contact me@clint.network


Type-DB is a serialized, in-memory and strongly typed database system developed in C#.

**Official Type-DB Documentation:** https://type-db.gitbook.io/overview

**Gitter Chat Room:** https://gitter.im/typedb-core

**Travis Status:** https://travis-ci.com/Type-DB/Core

[![Build Status](https://travis-ci.com/Type-DB/Core.svg?branch=development)](https://travis-ci.com/Type-DB/Core)
![](https://badges.gitter.im/typedb-core.png)
![](https://img.shields.io/badge/NuGet-0.0.0.152-blue.svg)

### What is Type-DB ?

Type-DB.Core is a library that allows you to store object locally (to use by example in an embedded project) or remotely in a Type-DB Server (see [Standalone or Remote Mode](#standalone-or-remote-mode))

I made Type-DB to meet the needs of any developer whether for an embedded project, to implement a cache system, or simply when you need a cool database, it is certain that you will love his modularity.

### Basic Usage

If you need a simple C# snippet to understand what is Type-DB, take a look here:
```csharp
var tdb = new TypeDB.Core(TypeDB.Mode.Standalone).Connect();

var testDatabase = tdb.OpenDatabase("test");

testDatabase.KeyValue.Set<int>("NumberOfCars", 28);
testDatabase.KeyValue.Increment<int>("NumberOfCars", 2);
var numberOfCars = testDatabase.KeyValue.Get<int>("NumberOfCars");
// numberOfCars equal to 30

testDatabase.Flush();
```

### Standalone or Remote Mode

Two operating modes are available on Type-DB, the `Standalone` mode that does not require the use of a server, which is very convenient for embedded projects, and the `Remote` mode that is coupled with a Type-DB Server.

Using Example in a Remote Mode:

```csharp
var tdb = new TypeDB.Core(TypeDB.Mode.Remote)
    .Configure(new TypeDB.Configuration
    {
        Host = "localhost",
        Port = 777
    })
    .Connect();
```

## Type-DB Features

#### Authentication Methods
There are three authentication methods:

- Anonymous, it's the default authentication method

- Basic, it's the authentication method with an username and a password

- Token, that use a JWT token

#### Encryption

All objects can be encrypted in AES, and decrypted on the fly.

#### Database Sealing

When a database is sealed, it's impossible to access to the object, the database must be unsealed before.

#### Signed Objects

Any object stored in a database can be signed.

#### Persistence

By default, an instance run in-memory, by using `UsePersistence()` you can automatically make a local-copy of this instance in a temporary file.

#### Triggers
You can easily set triggers to your Type-DB database, when you make the operation of your choice and run an associated callback.






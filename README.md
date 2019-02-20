# About Type-DB.Core

Type-DB is a serialized, in-memory and strongly typed database system developed in C#.

**Official Type-DB Documentation:** <https://type-db.gitbook.io/overview>

**Gitter Chat Room:** <https://gitter.im/typedb-core>

**NuGet Package:** <https://www.nuget.org/packages/Type-DB.Core>

[![Build Status](https://travis-ci.org/Type-DB/Core.svg?branch=development)](https://travis-ci.org/Type-DB/Core)
[![Gitter Chat](https://badges.gitter.im/typedb-core.png)](https://gitter.im/typedb-core)
[![NuGet](https://img.shields.io/badge/NuGet-0.0.0.402-blue.svg)](https://www.nuget.org/packages/Type-DB.Core)

## What is Type-DB

Type-DB is made to meet the needs of any developer whether for an embedded project, to implement a cache system, or simply when you need a cool database, it is certain that you will love his modularity.

## Key-Object Concept instead of Key-Value

The Core Concept of Type-DB is to replace the old concept of KeyValue dictionnaries with a new KeyObject system, that allows you to store any .Net Objects directly into a Type-DB Instance (see [Standalone or Remote Mode](#standalone-or-remote-mode))

## Basic Usage

If you need a simple C# snippet to understand what is Type-DB, take a look here:

```csharp
using (var tdb = new TypeDB.Core(Mode.Standalone).Connect())
using(var db = tdb.OpenDatabase("test", true))
{
    db.Set<int>("NumberOfCars", 28);
    var numberOfCars = db.Get<int>("NumberOfCars");
    Console.WriteLine(numberOfCars);
}
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

### Type-DB Features

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

# DewMySQLClient Middleware
If you need a way to set your ASP NET CORE website/api service with a MySQL Database, this middleware can help you.
However this works with [MySQLClient](https://github.com/andreabbondanza/MySQLClient) library.

# NOTE
You can use the version 1.0 for .net core (asp net core 1.x) or the version 2.0 for .net core 2.0 (asp net core 2)

# How to install

Simply go into NuGet package manager and search for DewMySQLClientMiddleware or use:
```Console
Install-Package DewMySQLClientMiddleware -Version 1.0.0
```
or
```Console
Install-Package DewMySQLClientMiddleware -Version 2.0.1
```
from package manager console.

# How it works

Now I'll show you how middleware works

## Defaults


This middleware is based on [MySQLClient](https://github.com/andreabbondanza/MySQLClient) library.

It use the __MySQLConnectionString__ type to initialize the database data.

The middleware has the __DewLocalizationMiddlewareOptions__ type. If you don't want customize anything you can just call the middleware with his Usexxx method.
However you can customize a bit the middleware with the options.
Here the type with default values
```c#
 public class MySQLConnectionString
{
    /// <summary>
    /// server address
    /// </summary>
    public readonly string Host;
    /// <summary>
    /// server port
    /// </summary>
    public readonly string Port;
    /// <summary>
    /// database name
    /// </summary>
    public readonly string Database;
    /// <summary>
    /// username
    /// </summary>
    public readonly string User;
    /// <summary>
    /// password
    /// </summary>
    public readonly string Password;
    /// <summary>
    /// Other flags
    /// </summary>
    public readonly string OtherFlags;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="host">server address</param>
    /// <param name="port">server port</param>
    /// <param name="database">database name</param>
    /// <param name="user">username</param>
    /// <param name="password">password</param>
    public MySQLConnectionString(string host, string port, string database, string user, string password, string otherFlags = null)
    {
        this.Host = host;
        this.Port = port;
        this.Database = database;
        this.User = user;
        this.Password = password;
        this.OtherFlags = otherFlags;
    }
    /// <summary>
    /// Return the current parameters connection string
    /// </summary>
    /// <returns></returns>
    public string GetConnectionString()
    {
        return @"host=" + this.Host + ";port=" + this.Port + ";user id=" + this.User + ";password=" + this.Password + ";database=" + this.Database + ";" + OtherFlags;
    }
}
```
## How add middleware to pipeline

```c#
app.UseDewMySQLClient(new MySQLConnectionString("myhost",
                                                "3066",
                                                "testdb",
                                                "user",
                                                "password",
                                                "UseAffectedRows=False"),//other flags
                                                "prefix_");
```

Remember to add the:
```c#
using DewCore.AspNetCore.Middlewares;
```
to use the UseDewLocalizationMiddleware method

## Get database's data to work with it

To see how work with database, check: [MySQLClient](https://github.com/andreabbondanza/MySQLClient)

If you want use it, you need to remember to add this into _using_ section:

```c#
using DewCore.AspNetCore.Middlewares;
```

After that, in the controller.

```c#
//a method into a controller
public IActionResult About()
{
    //Get database data from HTTPCONTEXT
    var connectionString = HttpContext.GetMySQLConnectionString();
    var tablePrefix = HttpContext.GetMySQLTablePrefix();
    //we have a custom model
    var model = new MyModel();
    //set database data to model
    model.SetDatabaseConnectionData(connectionString, tablePrefix);
    //get stuff
    var users = await model.GetUsers();        
    return View(users);
}
```

However you can get directly data from httpcontext item if you want. The keys are:

- DewDatabaseConnectionString
- DewDatabaseTablePrefix
## Note 
This middleware is strongly dependend from [MySQLClient](https://github.com/andreabbondanza/MySQLClient)
## NuGet
You can find it on nuget with the name [DewMySQLClientMiddleware ](https://www.nuget.org/packages/DewMySQLClientMiddleware/)

## About
[Andrea Vincenzo Abbondanza](http://www.andrewdev.eu)

## Donate
[Help me to grow up, if you want](https://payPal.me/andreabbondanza)
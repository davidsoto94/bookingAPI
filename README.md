# bookingAPI
¿What the API uses for DB?
-the API Uses Entity Framework to create the tables in SQL Server, in the case that a new database needs to be generated should you use the command line: 
"dotnet ef migration NameOfMigration", next, "dotnet ef database update", the Tables with the relations should be created. If there is an Error, check appsettings
for the connection string to ensure that is correct.

The Implementation was helped by this article to explain and implement JWT https://juandavid8a.github.io/tutoriales/2021/07/01/entity-user-net-core-5/ taking changes for the case.

¿What Tables does de API use?
-The Tables are de ASP.NET Entity Framework issue to authentication and authorization, join with a Room Table, in this case is only one room, but can increase in case that is necesary, and a reservation Table that has 2 foreign keys, one for the user, and another for the room.

¿Where can i see the methods?
-In the application Swagger, the service is include in development environment insidel the url given by iss express /swagger.


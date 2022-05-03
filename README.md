# bookingAPI
¿What the API uses for DB?
-the API Uses Entity Framework to create the tables in SQL Server, in the case that a new database needs to be generated should you use the command line: 
"dotnet ef migration NameOfMigration", next, "dotnet ef database update", the Tables with the relations should be created. If there is an Error, check appsettings
for the connection string to ensure that is correct.

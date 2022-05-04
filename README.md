# bookingAPI
¿What the API uses for DB?
-The API Uses Entity Framework to create the tables in SQL Server, in the case that a new database needs to be generated should you use the command line: 
"dotnet ef migration NameOfMigration", next, "dotnet ef database update", the Tables with the relations should be created. If there is an Error, check appsettings
for the connection string to ensure that is correct.

The Implementation was helped by this article to explain and implement JWT https://juandavid8a.github.io/tutoriales/2021/07/01/entity-user-net-core-5/ taking changes for the case.

¿What Tables does the API use?
-The Tables are the ASP.NET Entity Framework issue to authentication and authorization, join with a Room Table, in this case is only 1 room, but can increase in case that is necessary, and a reservation Table that has 2 foreign keys, one for the user, and another for the room.

¿Where can I see the methods?
-In the application Swagger, the service is include in development environment inside the url given by issexpress /swagger.

The API have, inside the Kubernetes folder in booking, 2 .yaml files to configure 2 services, one for sql server, and another for the booking API.

Due to a lack of time, I couldn't make an image with the pre-loaded database, however, in the Kubernetes service I left it to connect throught localhost connection so it could be populated in development environment with the "dotnet ef database update" command in powershell inside the booking Folder.

Once the services are alive, it assign a volume that persists even if the pods restart or directly die.

For informational purposes here the are all the API open methods:

POST /Register: To generate an user to Login.

{
  "username": "string",
  "email": "user@example.com",
  "password": "string"
}

POST /Login:    Gives back a token to the successful login attempt, this token must be copied and put the word Bearer, and space, and the token to work.


{
  "username": "string",
  "password": "string"
}


GET /Reservations/Rooms:  Show all the rooms created, in this case should be only one, but, since it is a different table in the database could be any quantity.


GET /Reservations/Rooms/{id}: Show an specific room.


GET /Ocuppation:   Returns all the reservations made without the id.


GET /Reservations/{id}:  Returns an specific reservation with the id, that can be get throught /Reservations/MyReservation that only can be seen if you are logged in.


GET /Reservations/MyReservation: Can only be access with the token given by the login, shows the reservation, if there is any, for the specific user.


POST /Reservations: Can only be access with the token given by the login, creates a reservations and returns the reservation with the id.


{
  "reservationId": 0,
  "roomId": 0,
  "initialDate": "2022-05-04T01:50:46.794Z",
  "finalDate": "2022-05-04T01:50:46.794Z"
}


PUT /Reservations:    Can only be access with the token given by the login, modifies, if there is any availability, the room reservation.


{
  "id": 0,
  "roomId": 0,
  "initialDate": "2022-05-04T01:51:51.408Z",
  "finalDate": "2022-05-04T01:51:51.408Z"
}


DELETE /Reservations/{id}:    Can only be access with the token given by the login, deletes the reservation given the id, if it's not that user, the method returns error and a message.


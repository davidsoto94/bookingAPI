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

The API have, inside the Kubernetes folder in booking, two .yaml files to configure two services, one for sql server, and another for the booking API.

Due to a lack of time i clouldn't make an image with the pre-loaded database, however, in the kubernet service i left it to connect throug localhost connection so it could be populated in development envirement with the dotnet ef migration NameOfMigration command in powershell inside the booking Folder.

once the services are alive it assign a volume that persists even if the pods restart or directly die.

for informational pourpuses here the are all the API open methods:

POST /Register: to generate an user to Login
{
  "username": "string",
  "email": "user@example.com",
  "password": "string"
}
POST /Login:    gives back a token to the succesfull login attempt, this token must be copied and put the word Bearer and space and the token to work
{
  "username": "string",
  "password": "string"
}
GET /Reservations/Rooms:  show all the rooms created, in this case should be only one, but, since it is a diferent table in the database could be any cuantity.
GET /Reservations/Rooms/{id}: show an specific room.
GET /Ocuppation:   Returns all the reservations made without the id.
GET /Reservations/{id}:  returns an specific reservation with the id, that can be get througt /Reservations/MyReservation that only can be seen if you are logged in
GET /Reservations/MyReservation: can only be access with the token given by the login, shows the reservation, if therer is any, for the specific user
POST /Reservations: can only be access with the token given by the login, creates a reservations and returns the reservation with the id
{
  "reservationId": 0,
  "roomId": 0,
  "initialDate": "2022-05-04T01:50:46.794Z",
  "finalDate": "2022-05-04T01:50:46.794Z"
}
PUT /Reservations:    can only be access with the token given by the login, modifies, if there is any avaliability, the room reservation.
{
  "id": 0,
  "roomId": 0,
  "initialDate": "2022-05-04T01:51:51.408Z",
  "finalDate": "2022-05-04T01:51:51.408Z"
}
DELETE /Reservations/{id}:    can only be access with the token given by the login,deletes the reservation given the id, if it's not that user reservation returns error and a message


using Booking.Data;
using Booking.Models;

namespace Booking.Repositories
{
    public class SqlDbRepository : IRoomReservationRepository
    {
        private readonly ApplicationDbContext sqlClient;
        public SqlDbRepository(ApplicationDbContext sqlClient)
        {
            this.sqlClient = sqlClient;
        }

        public Reservation CreateReservation(Reservation reservation)
        {
            sqlClient.Reservations.Add(reservation);
            sqlClient.SaveChanges();
            Reservation reservationId = (from r in sqlClient.Reservations
                                         where r.InitialDate == reservation.InitialDate &&
                                         r.FinalDate == reservation.FinalDate && r.RoomId == reservation.RoomId
                                         select r).FirstOrDefault();
            return reservationId;
        }

        public void CreateRoom(Room room)
        {
            sqlClient.Rooms.Add(room);
            sqlClient.SaveChanges();
        }

        public Reservation GetReservation(int id)
        {
            Reservation reservation = (from r in sqlClient.Reservations
                                       where r.Id == id
                                       select r).FirstOrDefault();
            return reservation;
        }

        public IEnumerable<Room> GetRooms()
        {
            return sqlClient.Rooms.ToList();
        }

        public Room GetRoom(int id)
        {
            return sqlClient.Rooms.Where(room => room.Id == id).SingleOrDefault();
        }

        public IEnumerable<Reservation> GetOccupacy()
        {
            var result = sqlClient.Reservations.Where(reservation => reservation.FinalDate > DateTime.Today);
            return result;
        }

        //Get the reservation per User

        public IEnumerable<Reservation> GetReservation(string UserId)
        {
            var result = (from r in sqlClient.Reservations
                          where r.UserId == UserId && r.FinalDate > DateTime.Today
                          select r);
            return result;
        }

        public void UpdateReservation(Reservation reservation)
        {
            sqlClient.Entry(sqlClient.Reservations.FirstOrDefault(x => x.Id == reservation.Id)).CurrentValues.SetValues(reservation);
            sqlClient.SaveChanges();
        }

        public void DeleteReservation(Reservation reservation)
        {
            sqlClient.Reservations.Remove(reservation);
            sqlClient.SaveChanges();
        }
    }
}
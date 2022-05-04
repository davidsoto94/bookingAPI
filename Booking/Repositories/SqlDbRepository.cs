using Booking.Data;
using Booking.Models;
using Microsoft.EntityFrameworkCore;

namespace Booking.Repositories
{
    public class SqlDbRepository : IRoomReservationRepository
    {
        private readonly ApplicationDbContext sqlClient;
        public SqlDbRepository(ApplicationDbContext sqlClient)
        {
            this.sqlClient = sqlClient;
        }

        public async Task<Reservation> CreateReservationAsync(Reservation reservation)
        {
            await sqlClient.Reservations.AddAsync(reservation);
            await sqlClient.SaveChangesAsync();
            Reservation reservationId =  await (from r in sqlClient.Reservations
                                         where r.InitialDate == reservation.InitialDate &&
                                         r.FinalDate == reservation.FinalDate && r.RoomId == reservation.RoomId
                                         select r).FirstOrDefaultAsync();
            return reservationId;   
        }

        public async Task CreateRoomAsync(Room room)
        {
            await sqlClient.Rooms.AddAsync(room);
            await sqlClient.SaveChangesAsync();

        }

        public async Task<Reservation> GetReservationAsync(int id)
        {
            Reservation reservation = await (from r in  sqlClient.Reservations
                                       where r.Id == id
                                       select r).FirstOrDefaultAsync();
            return reservation;
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync()
        {
            return await sqlClient.Rooms.ToListAsync();
        }

        public async Task<Room> GetRoomAsync(int id)
        {
            return await sqlClient.Rooms.Where(room => room.Id == id).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Reservation>> GetOccupacyAsync()
        {
            var result = await sqlClient.Reservations.Where(reservation => reservation.FinalDate > DateTime.Today).ToListAsync();
            return result;
        }

        //Get the reservation per User

        public async Task<IEnumerable<Reservation>> GetReservationAsync(string UserId)
        {
            var result = await (from r in sqlClient.Reservations
                          where r.UserId == UserId && r.FinalDate > DateTime.Today
                          select r).ToListAsync();
            return result;
        }

        public async Task UpdateReservationAsync(Reservation reservation)
        {
            sqlClient.Entry(sqlClient.Reservations.FirstOrDefaultAsync(x => x.Id == reservation.Id)).CurrentValues.SetValues(reservation);
            await sqlClient.SaveChangesAsync();
        }

        public async Task DeleteReservationAsync(Reservation reservation)
        {
            sqlClient.Reservations.Remove(reservation);
            await sqlClient.SaveChangesAsync();
        }
    }
}
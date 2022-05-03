using Booking.Models;

namespace Booking.Repositories
{
    public interface IRoomReservationRepository
    {
        IEnumerable<Room> GetRooms();
        Room GetRoom(int id);
        void CreateRoom(Room room);
        Reservation GetReservation(int id);
        IEnumerable<Reservation> GetReservation(string UserId);
        IEnumerable<Reservation> GetOccupacy();
        Reservation CreateReservation(Reservation reservation);

        void UpdateReservation(Reservation reservation);
        void DeleteReservation(Reservation reservation);
        
    }
}
using Booking.Models;

namespace Booking.Repositories
{
    public interface IRoomReservationRepository
    {
        Task<IEnumerable<Room>> GetRoomsAsync();
        Task<Room> GetRoomAsync(int id);
        Task CreateRoomAsync(Room room);
        Task<Reservation> GetReservationAsync(int id);
        Task<IEnumerable<Reservation>> GetReservationAsync(string UserId);
        Task<IEnumerable<Reservation>> GetOccupacyAsync();
        Task<Reservation> CreateReservationAsync(Reservation reservation);

        Task UpdateReservationAsync(Reservation reservation);
        Task DeleteReservationAsync(Reservation reservation);
        
    }
}
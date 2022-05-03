using Booking.Dtos;
using Booking.Models;

namespace Booking
{
    public static class Extensions{
        public static RoomDto AsDto(this Room room){
            return new RoomDto{Id=room.Id,MaxOccupants=room.MaxOccupants};
        }
        public static ReservationsDto AsDto(this Reservation reservation){
            return  new ReservationsDto()
            {
                reservationId=reservation.Id,
                RoomId = reservation.RoomId,
                InitialDate = reservation.InitialDate,
                FinalDate = reservation.FinalDate,            
            };
        }
        
    }
}
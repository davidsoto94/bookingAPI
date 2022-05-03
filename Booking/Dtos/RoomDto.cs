using System.ComponentModel.DataAnnotations;

namespace Booking.Dtos
{
    public class RoomDto {

        public int Id{get;set;}
        [Range(1,20)]
        public int MaxOccupants { get;set; }
    }
}
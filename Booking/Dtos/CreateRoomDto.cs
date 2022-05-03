using System.ComponentModel.DataAnnotations;

namespace Booking.Dtos
{
    public class CreateRoomDto {


        [Range(1,20)]
        public int MaxOccupants { get;set; }
    }
}
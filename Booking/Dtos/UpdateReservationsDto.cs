using System.ComponentModel.DataAnnotations;

namespace Booking.Dtos{
    public class UpdateReservationsDto
    {
        [Required]
        public int Id{get;set;}
        public int RoomId{get;set;}
        public DateTime InitialDate{get;set;}
        public DateTime FinalDate{get;set;}
    }
}
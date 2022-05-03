namespace Booking.Dtos{
    public class ReservationsDto
    {
        public int reservationId{get;set;}
        public int RoomId{get;set;}
        public DateTime InitialDate{get;set;}
        public DateTime FinalDate{get;set;}
    }
}
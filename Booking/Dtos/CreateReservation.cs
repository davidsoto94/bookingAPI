using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


//Models Used to Create Database through Entity Framework(EF) with Foreign Key in SQL Server
namespace Booking.Dtos{
    public class CreateReservationDto
    {


        [Required]
        [Range(1,10000)]
        public int RoomId{get;set;}
        [Required]
         public DateTime InitialDate{get;set;}
         [Required]
         public DateTime FinalDate{get;set;}

    }
}
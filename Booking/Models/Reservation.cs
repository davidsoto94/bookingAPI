using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


//Models Used to Create Database through Entity Framework(EF) with Foreign Key in SQL Server
namespace Booking.Models{
    public class Reservation 
    {
        [Key]
        public int Id{get;set;}
        [ForeignKey("ApplicationUser")]
        public string UserId {get;set;}
        public ApplicationUser  ApplicationUser {get;set;}

        [ForeignKey("Room")]
        public int RoomId{get;set;}
        public Room Room{get;set;}
        public DateTime InitialDate{get;set;}
        public DateTime FinalDate{get;set;}


    }
}
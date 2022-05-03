using Booking.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//Model for de Application User to use Entity Framework Authorization
namespace Booking.Data
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            :base(options)
        {

        }
       
        public DbSet<Room> Rooms{get;set;}
        public DbSet<Reservation> Reservations{get;set;}
        
       

    }
}
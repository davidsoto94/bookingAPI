using Microsoft.AspNetCore.Mvc;
using Booking.Repositories;
using Booking.Models;
using Microsoft.AspNetCore.Authorization;
using Booking.Dtos;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Booking.Controllers
{
    [ApiController]
    [Route("Reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly IRoomReservationRepository repository;
        public ReservationController(IRoomReservationRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.repository = repository;
        }

        //GET:/Reservation/Rooms
        [HttpGet]
        [Route("Rooms")]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRoomsAsync()
        {
            var rooms =(await  repository.GetRoomsAsync()).Select(room => room.AsDto());
            if (rooms.Count() <= 0)
            {
                return NotFound();
            }
            return rooms.ToList();
        }


        [HttpGet]
        [Route("Rooms/{id}")]

        public async Task<ActionResult<RoomDto>> GetRoomsAsync(int id)
        {

            var room = await repository.GetRoomAsync(id);
            if (room is null)
            {
                return NotFound();
            }
            return room.AsDto();
        }
        //Class to Create new Rooms, however, in this case wont be implemented unless there is roles in the future
        /*
        [HttpPost]
        [Route("Rooms")]
        public async Task<ActionResult<RoomDto>> CreateRoomAsync(CreateRoomDto roomDto)
        {
           Room room =new(){
               MaxOccupants=roomDto.MaxOccupants
           };
           repository.CreateRoomAsync(room);
           return room.AsDto();
        }*/

        [HttpGet]
        [Route("/Ocuppation")]
        public async Task<ActionResult<IEnumerable<ReservationsDto>>> GetOcupattionAsync()
        {
            var reservations = (await repository.GetOccupacyAsync()).Select(reservation=>new ReservationsDto()
            {
                RoomId = reservation.RoomId,
                InitialDate = reservation.InitialDate,
                FinalDate = reservation.FinalDate,            
            });
            if(reservations.Count()<=0){
                return NotFound();
            }
            return reservations.ToList();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ReservationsDto>> GetReservationsAsync(int id)
        {
            Reservation reservation =await repository.GetReservationAsync(id);
            if(reservation is null){
                return NotFound();
            }
            return reservation.AsDto();
        }

        [Authorize]
        [HttpGet]
        [Route("MyReservation")]
        public async Task<ActionResult<IEnumerable<ReservationsDto>>> GetMyReservationAsync()
        {
            var reservation =(await repository.GetReservationAsync(userManager.Users.ToList()[0].Id)).Select(reservation=>reservation.AsDto());
            if(reservation.Count() <= 0){
                return NotFound();
            }
            return reservation.ToList();
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReservationsDto>> CreateReservationsAsync(CreateReservationDto reservationDto)
        {
           DateTime initialDate=reservationDto.InitialDate.Date;
           DateTime finalDate=reservationDto.FinalDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
           bool overlap=false;
           foreach(var reservationDate in (await repository.GetOccupacyAsync()).Select(reservation=>reservation.AsDto())){
               overlap = initialDate < reservationDate.FinalDate && reservationDate.InitialDate < finalDate; 
           }
           if(overlap){
                return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "The dates overlap with a previus reservation" });
           }
           if(await repository.GetRoomAsync(reservationDto.RoomId) is null){
                return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "Please enter a valid room id" });
           }
           if(finalDate <=initialDate){
               return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "The initial date cannot be the same or lower as the final date" });
           }
           if((finalDate-initialDate).TotalDays>3){
               return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "You cannot reserve more than 3 days" });
           }
           if(finalDate>DateTime.Today.Date.AddMonths(1).AddDays(1)){
               return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "You cannot reserve more than 30 days in advance" });
           }
           Reservation reservation = new()
            {
                UserId=userManager.Users.ToList()[0].Id,
                RoomId=reservationDto.RoomId,
                InitialDate=initialDate,
                FinalDate= finalDate
            };
            var result=await repository.CreateReservationAsync(reservation);
            return CreatedAtAction(nameof(GetReservationsAsync), new{id=result.Id},result.AsDto());
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateReservationAsync(UpdateReservationsDto reservationDto)
        {
           DateTime initialDate=reservationDto.InitialDate.Date;
           DateTime finalDate=reservationDto.FinalDate.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
           if(await repository.GetReservationAsync(reservationDto.Id) is null){
                return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "The reservation Id is not valid" });
           }
           
           bool overlap=false;
           foreach(var reservationDate in await repository.GetOccupacyAsync()){
               if(reservationDate.Id!=reservationDto.Id){
                   overlap = initialDate < reservationDate.FinalDate && reservationDate.InitialDate < finalDate; 
               }
               
           }
           if(overlap){
                return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "The dates overlap with a previus reservation" });
           }
           if(repository.GetRoomAsync(reservationDto.RoomId) is null){
                return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "Please enter a valid room id" });
           }
           if(finalDate <=initialDate){
               return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "The initial date cannot be the same or lower as the final date" });
           }
           if((finalDate-initialDate).TotalDays>3){
               return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "You cannot reserve more than 3 days" });
           }
           if(finalDate>DateTime.Today.Date.AddMonths(1).AddDays(1)){
               return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "You cannot reserve more than 30 days in advance" });
           }
           Reservation reservation = new()
            {
                Id=reservationDto.Id,
                UserId=userManager.Users.ToList()[0].Id,
                RoomId=reservationDto.RoomId,
                InitialDate=initialDate,
                FinalDate= finalDate
            };
            await repository.UpdateReservationAsync(reservation);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> DeleteReservationAsync(int Id)
        {
           var reservation= await repository.GetReservationAsync(Id);
           if( reservation is null){
                return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "The reservation Id is not valid" });
           } 
           if(reservation.UserId!=userManager.Users.ToList()[0].Id){
               return StatusCode(StatusCodes.Status406NotAcceptable, new Response 
               { Status = "Error", Message = "This user do not own this reservation" });
           }
            await repository.DeleteReservationAsync(reservation);
            return NoContent();
        }

    }
}
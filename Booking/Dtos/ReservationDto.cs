using System;
namespace Booking.Dtos
{
	public class ReservationDto
	{
		public string RoomType { get; set; } = string.Empty;
		public decimal Price { get; set; }
	}
}


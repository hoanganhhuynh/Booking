using System;
namespace Booking.Domain.Entities
{
	public class RoomEntity
	{
		public string RoomType { get; set; } = string.Empty;
		public int Sleeps { get; set; }
		public decimal Price { get; set; }
		public int NumberOfRooms { get; set; }
	}
}


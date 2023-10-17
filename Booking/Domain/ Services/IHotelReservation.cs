using System;
using Booking.Domain.Entities;
using Booking.Dtos;

namespace Booking.Domain.Services
{
	public interface IHotelReservation
	{
        List<ReservationDto> CreateReservation(int personNumber);
		bool IsEnoughRoom(int personNumber);
	}
}


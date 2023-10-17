using System;
using Booking.Domain.Entities;
using Booking.Dtos;

namespace Booking.Domain.Services
{
	public interface IHotelReservation
	{
        List<ReservationDto> CteateReservation(int personNumber);
		bool IsEnoughRoom(int personNumber);
	}
}


using System;
using System.Collections.Generic;
using System.Linq;
using Booking.Domain.Entities;
using Booking.Domain.Services;
using Booking.Dtos;
using Booking.Utils;

namespace Booking.Infrastructure.Services
{
	public class HotelReservation : IHotelReservation
    {
        public static List<HotelEntity> rooms = new List<HotelEntity>
        {
            new HotelEntity { RoomType = "Single", Sleeps = 1, NumberOfRooms = 2, Price = 30 },
            new HotelEntity { RoomType = "Double", Sleeps = 2, NumberOfRooms = 3, Price = 50 },
            new HotelEntity { RoomType = "Family", Sleeps = 4, NumberOfRooms = 1, Price = 85 }
        };
        public HotelReservation()
		{
		}

        public List<ReservationDto> CteateReservation(int personNumber)
        {
            int remainingPerson = personNumber;
            var result = GetFamilyRooms(remainingPerson);

            remainingPerson = personNumber - (result.Count() * 4);

            if (remainingPerson >= 2)
            {
                var doubleRooms = GetDoubleRooms(remainingPerson);
                if (doubleRooms.Count > 0)
                {
                    result.AddRange(doubleRooms);
                    remainingPerson = remainingPerson - (doubleRooms.Count() * 2);
                }
            }

            if (remainingPerson >= 1)
            {
                var singleRooms = GetSingleRooms(remainingPerson);
                if (singleRooms.Count > 0)
                {
                    result.AddRange(singleRooms);
                    remainingPerson = remainingPerson - singleRooms.Count();
                }
            }

            return result;
        }

        private List<ReservationDto> GetFamilyRooms(int personNumber)
        {
            var familyRoom = rooms.FirstOrDefault(r => r.RoomType == RoomType.Family.ToString());
            var result = new List<ReservationDto>();
            var numberOfRoom = personNumber / 4;
            if (numberOfRoom > familyRoom!.NumberOfRooms)
            {
                numberOfRoom = familyRoom.NumberOfRooms;
            }

            ReservationDto[] arrayValue = new ReservationDto[numberOfRoom];
            Array.Fill<ReservationDto>(arrayValue, new ReservationDto { RoomType = familyRoom.RoomType, Price = familyRoom.Price });
            familyRoom.NumberOfRooms = familyRoom.NumberOfRooms - numberOfRoom;

            result.AddRange(arrayValue.ToList());
            return result;
        }

        private List<ReservationDto> GetDoubleRooms(int personNumber)
        {
            var doubleRoom = rooms.FirstOrDefault(r => r.RoomType == RoomType.Double.ToString());
            var result = new List<ReservationDto>();
            var numberOfRoom = personNumber / 2;
            if (numberOfRoom > doubleRoom!.NumberOfRooms)
            {
                numberOfRoom = doubleRoom.NumberOfRooms;
            }
            ReservationDto[] arrayValue = new ReservationDto[numberOfRoom];
            Array.Fill<ReservationDto>(arrayValue, new ReservationDto { RoomType = doubleRoom.RoomType, Price = doubleRoom.Price });
            doubleRoom.NumberOfRooms = doubleRoom.NumberOfRooms - numberOfRoom;

            result.AddRange(arrayValue.ToList());
            return result;
        }

        private List<ReservationDto> GetSingleRooms(int personNumber)
        {
            var singleRoom = rooms.FirstOrDefault(r => r.RoomType == RoomType.Single.ToString());
            var result = new List<ReservationDto>();
            var numberOfRoom = personNumber;
            ReservationDto[] arrayValue = new ReservationDto[personNumber];
            Array.Fill<ReservationDto>(arrayValue, new ReservationDto { RoomType = singleRoom!.RoomType, Price = singleRoom.Price });
            singleRoom.NumberOfRooms = singleRoom.NumberOfRooms - numberOfRoom;

            result.AddRange(arrayValue.ToList());
            return result;
        }

        public bool IsEnoughRoom(int personNumber)
        {
            var allPersonAvailable = 0;
            foreach (var room in rooms)
            {
                allPersonAvailable = allPersonAvailable + (room.NumberOfRooms * room.Sleeps);
            }

            return allPersonAvailable >= personNumber;
        }
    }
}


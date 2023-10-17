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
        public static Dictionary<RoomType, RoomEntity> rooms = new Dictionary<RoomType, RoomEntity>
        {
            { RoomType.Single, new RoomEntity { RoomType = "Single", Sleeps = 1, NumberOfRooms = 2, Price = 30 } },
            { RoomType.Double, new RoomEntity { RoomType = "Double", Sleeps = 2, NumberOfRooms = 3, Price = 50 } },
            { RoomType.Family, new RoomEntity { RoomType = "Family", Sleeps = 4, NumberOfRooms = 1, Price = 85 } },
        };
        
        public List<ReservationDto> CreateReservation(int personNumber)
        {
            int remainingPerson = personNumber;
            var result = GetFamilyRooms(ref remainingPerson);

            var doubleRooms = GetDoubleRooms(ref remainingPerson);
            if (doubleRooms.Count > 0)
            {
                result.AddRange(doubleRooms);
            }

            var singleRooms = GetSingleRooms(ref remainingPerson);
            if (singleRooms.Count > 0)
            {
                result.AddRange(singleRooms);
            }

            return result;
        }

        private List<ReservationDto> GetFamilyRooms(ref int remainingPersonNumber)
        {
            var familyRoom = rooms[RoomType.Family];
            
            var numberOfRoom = remainingPersonNumber / 4;
            var result = GetRooms(numberOfRoom, familyRoom);

            remainingPersonNumber = remainingPersonNumber - (result.Count * Constants.MAX_SLEEPS_FAMILY_ROOM);
            return result;
        }

        private List<ReservationDto> GetDoubleRooms(ref int remainingPersonNumber)
        {
            var doubleRoom = rooms[RoomType.Double];
            var numberOfRoom = remainingPersonNumber / 2;
            
            var result = GetRooms(numberOfRoom, doubleRoom);
            remainingPersonNumber = remainingPersonNumber - (result.Count * Constants.MAX_SLEEPS_DOUBLE_ROOM);
            return result;
        }

        private List<ReservationDto> GetSingleRooms(ref int remainingPersonNumber)
        {
            var singleRoom = rooms[RoomType.Single];
            var numberOfRoom = remainingPersonNumber;

            var result = GetRooms(numberOfRoom, singleRoom);
            remainingPersonNumber = remainingPersonNumber - (result.Count * Constants.MAX_SLEEPS_SINGLE_ROOM);
            return result;
        }

        private List<ReservationDto> GetRooms(int numberOfRoom, RoomEntity room)
        {
            if (numberOfRoom == 0)
            {
                return new List<ReservationDto>();
            }
            numberOfRoom = numberOfRoom >= room!.NumberOfRooms
                ? room.NumberOfRooms
                : numberOfRoom;
            ReservationDto[] arrayValue = new ReservationDto[numberOfRoom];
            Array.Fill<ReservationDto>(arrayValue, new ReservationDto { RoomType = room.RoomType, Price = room.Price });
            room.NumberOfRooms = room.NumberOfRooms - arrayValue.Length;

            return arrayValue.ToList();
        }

        public bool IsEnoughRoom(int personNumber)
        {
            var allPersonAvailable = 0;
            foreach (var room in rooms)
            {
                allPersonAvailable = allPersonAvailable + (room.Value.NumberOfRooms * room.Value.Sleeps);
            }

            return allPersonAvailable >= personNumber;
        }
    }
}
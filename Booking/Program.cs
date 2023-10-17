using Booking.Domain.Services;
using Booking.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;


public class Program
{
    public static void Main(string[] args)
    {
        //setup our DI
        var serviceProvider = new ServiceCollection()
            
            .AddTransient<IHotelReservation, HotelReservation>()
            .BuildServiceProvider();

        //do the actual work here
        var action = "";
        do
        {
            Console.Write("Please enter number of guests");
            int numberPerson = 0;
            bool isValid = int.TryParse(Console.ReadLine(), out numberPerson);

            // Loops until user enters number from 1 - 7
            while (!isValid || numberPerson == 0)
            {
                
                Console.Write("  Invalid value. Please enter again ");
                isValid = int.TryParse(Console.ReadLine(), out numberPerson);
            }

            var reservationService = serviceProvider.GetService<IHotelReservation>();
            if (reservationService != null)
            {

                var result = GetBookingConfirmation(numberPerson, reservationService);
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.Write("Do you want to continue booking? Enter Y if you want to continue. Enter any character to stop booking.");
        }
        while (action == "Y");

        
    }

    private static string GetBookingConfirmation(int numberPerson, IHotelReservation hotelReservation)
    {
        var hasAvailableRoom = hotelReservation.IsEnoughRoom(7);
        if (hasAvailableRoom)
        {
            var reservations = hotelReservation.CteateReservation(7);
            return $"{string.Join(" ", reservations.Select(r => r.RoomType))} {reservations.Select(r => r.Price).Sum()}";
            
        }
        else
        {
            return "No Option";
        }
    }
}
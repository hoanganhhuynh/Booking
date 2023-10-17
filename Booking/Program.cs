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
            Console.Write("Please enter number of guests: ");
            int numberPerson = 0;
            bool isValid = int.TryParse(Console.ReadLine(), out numberPerson);

            while (!isValid || numberPerson == 0)
            {
                
                Console.Write("  Invalid value. Please enter again ");
                isValid = int.TryParse(Console.ReadLine(), out numberPerson);
            }

            var reservationService = serviceProvider.GetService<IHotelReservation>();
            if (reservationService != null)
            {

                var result = GetBookingConfirmation(numberPerson, reservationService);
                Console.WriteLine(result);
            }
            else
            {
                Console.WriteLine("Error");
            }

            Console.Write("Do you want to continue booking? Enter Y if you want to continue. Enter any character to stop booking.");
            action = Console.ReadLine();
        }
        while (action?.ToUpper() == "Y");

        
    }

    private static string GetBookingConfirmation(int numberPerson, IHotelReservation hotelReservation)
    {
        var hasAvailableRoom = hotelReservation.IsEnoughRoom(numberPerson);
        if (hasAvailableRoom)
        {
            var reservations = hotelReservation.CteateReservation(numberPerson);
            return $"{string.Join(" ", reservations.Select(r => r.RoomType))} {reservations.Select(r => r.Price).Sum()}";
        }
        else
        {
            return "No Option";
        }
    }
}
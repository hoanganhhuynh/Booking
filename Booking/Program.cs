using System;
using Booking.Domain.Services;
using Booking.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = RegisterServices();
        var action = "";
        do
        {
            var numberPerson = GetNumberOfPerson();
            var bookingResult = GetBookingResult(numberPerson, serviceProvider);

            Console.WriteLine(bookingResult);
            Console.Write("Do you want to continue booking? Enter Y if you want to continue. Enter any character to stop booking.");
            action = Console.ReadLine();
        }
        while (action?.ToUpper() == "Y");
    }

    private static ServiceProvider RegisterServices()
    {
        var serviceProvider = new ServiceCollection()
            .AddTransient<IHotelReservation, HotelReservation>()
            .BuildServiceProvider();

        return serviceProvider;
    }

    private static int GetNumberOfPerson()
    {
        Console.Write("Please enter number of guests: ");
        int numberPerson = 0;
        bool isValid = int.TryParse(Console.ReadLine(), out numberPerson);

        while (!isValid || numberPerson == 0)
        {
            Console.Write("  Invalid value. Please enter again ");
            isValid = int.TryParse(Console.ReadLine(), out numberPerson);
        }
        return numberPerson;
    }

    private static string GetBookingResult(int numberPerson, ServiceProvider serviceProvider)
    {
        var reservationService = serviceProvider.GetService<IHotelReservation>();
        if (reservationService == null)
        {
            return "Service does not support";
        }

        var hasAvailableRoom = reservationService.IsEnoughRoom(numberPerson);
        if (hasAvailableRoom)
        {
            var reservations = reservationService.CreateReservation(numberPerson);
            return $"{string.Join(" ", reservations.Select(r => r.RoomType))} {reservations.Select(r => r.Price).Sum()}";
        }
        else
        {
            return "No Option";
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RestaurantReservation.Db;

namespace RestaurantReservation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
       .ConfigureAppConfiguration((hostingContext, config) =>
        {
           config.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
       })
       .ConfigureServices((context, services) =>
       {
           services.AddDbContext<RestaurantReservationDbContext>(options =>
               options.UseSqlServer(context.Configuration.GetConnectionString("RestaurantReservationDb")));

           services.AddTransient<TesterClass>();
       })
       .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var reservationTester = services.GetRequiredService<TesterClass>();
                await reservationTester.Test();
            }
        }
    }
}

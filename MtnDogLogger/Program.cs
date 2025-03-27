using SocketCANSharp;
using SocketCANSharp.Network;

namespace MtnDogLogger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddRequestDecompression();

            builder.Services.AddControllers();

            ConfigureSocketCAN();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();


            app.UseAuthorization();

            app.UseRequestDecompression();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureSocketCAN()
        {
            var allCan = CanNetworkInterface.GetAllInterfaces(false);

            foreach (var iface in allCan)
            {
                Console.WriteLine($"Name: {iface.Name}, Status: {iface.OperationalStatus}");
            }
        }
    }
}

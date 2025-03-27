using SocketCANSharp;
using SocketCANSharp.Network;
using SocketCANSharp.Network.Netlink;

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

           // app.Run();
        }

        private static void ConfigureSocketCAN()
        {
            var can0 = CanNetworkInterface.GetAllInterfaces(false).First();

            //foreach (var iface in allCan)
            //{
                Console.WriteLine($"Name: {can0.Name}, Status: {can0.OperationalStatus}");
            //}
            can0.SetLinkDown();

            Console.WriteLine($"Name: {can0.Name}, Status: {can0.OperationalStatus}");

            can0.BitTiming = new CanBitTiming() { BitRate = 1000000 };
            can0.CanControllerModeFlags = CanControllerModeFlags.CAN_CTRLMODE_LOOPBACK | CanControllerModeFlags.CAN_CTRLMODE_ONE_SHOT;
            can0.AutoRestartDelay = 5;
            can0.MaximumTransmissionUnit = SocketCanConstants.CAN_MTU;

            can0.SetLinkUp();

            Console.WriteLine($"Name: {can0.Name}, Status: {can0.OperationalStatus}");


            using (var socket = new RawCanSocket())
            {
                socket.Bind(can0);
                var bytes = socket.Read(out CanFrame frame);
                
                Console.WriteLine("Read Bytes:" + bytes);
                Console.WriteLine("CAN ID:" + frame.CanId);
                Console.WriteLine("Byte0:" + frame.Data[0]);
                Console.WriteLine("Byte1:" + frame.Data[1]);
                Console.WriteLine("Byte2:" + frame.Data[2]);
                Console.WriteLine("Byte3:" + frame.Data[3]);
                Console.WriteLine("Byte4:" + frame.Data[4]);
                Console.WriteLine("Byte5:" + frame.Data[5]);
                Console.WriteLine("Byte6:" + frame.Data[6]);
                Console.WriteLine("Byte7:" + frame.Data[7]);
                
            }
        }
    }
}

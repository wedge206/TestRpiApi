using SocketCANSharp;
using SocketCANSharp.Network;
using SocketCANSharp.Network.BroadcastManagement;
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
            var can0 = CanNetworkInterface.GetAllInterfaces(false).First(iface => iface.Name.Equals("can0"));

            //foreach (var iface in allCan)
            //{
                Console.WriteLine($"Name: {can0.Name}, Status: {can0.OperationalStatus}");
            //}
            can0.SetLinkDown();

            Console.WriteLine($"Name: {can0.Name}, Status: {can0.OperationalStatus}");

            can0.BitTiming = new CanBitTiming() { BitRate = 1000000 };
            //can0.CanControllerModeFlags = CanControllerModeFlags.CAN_CTRLMODE_3_SAMPLES | CanControllerModeFlags.CAN_CTRLMODE_CC_LEN8_DLC | CanControllerModeFlags.CAN_CTRLMODE_PRESUME_ACK;
            can0.AutoRestartDelay = 5;
            can0.MaximumTransmissionUnit = SocketCanConstants.CAN_MTU;

            can0.SetLinkUp();

            Console.WriteLine($"Name: {can0.Name}, Status: {can0.OperationalStatus}");

            using (var bcmSocket = new BcmCanSocket())
            {
                bcmSocket.Connect(can0);

                var canFrame = new CanFrame(0x333, new byte[] { 0xDE, 0xAD, 0xBE, 0xEF, 0xDE, 0xAD, 0xBE, 0xEF });
                var frames = new CanFrame[] { canFrame };
                var config = new BcmCyclicTxTaskConfiguration()
                {
                    Id = 0x333,
                    StartTimer = true,
                    SetInterval = true,
                    InitialIntervalConfiguration = new BcmInitialIntervalConfiguration(0, new BcmTimeval(0, 0)), // 10 messages at 5 ms
                    PostInitialInterval = new BcmTimeval(0, 100000), // Then at 100 ms
                    CopyCanIdInHeaderToEachCanFrame = true,
                };
                int nBytes = bcmSocket.CreateCyclicTransmissionTask(config, frames);
            }


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

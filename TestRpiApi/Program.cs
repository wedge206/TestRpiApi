using Peak.Can.Basic;

namespace TestRpiApi
{
    public class Program
    {
        static Worker canWorker = new Worker();
        static byte[] canBytes = new byte[8];


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();


            app.MapControllers();

            InitPcan();

            app.Run();
        }

        private static void InitPcan()
        {
            try
            {
                PcanStatus result = Api.Initialize(PcanChannel.Usb01, Bitrate.Pcan1000);

                Console.WriteLine("CAN Init Result: " + result.ToString());

                canWorker.MessageAvailable += OnCanMessageAvailable;
                canWorker.AddFilter(new FilteringCriterion()
                {
                    // TODO Add filters
                });
                canWorker.Start();
            }
            catch (Exception)
            {
                Console.WriteLine("handle point 1");
                throw;
            }
        }

        public static byte[] GetBytes()
        {
            return canBytes;
        }

        private static void OnCanMessageAvailable(object sender, MessageAvailableEventArgs e)
        {
            while (canWorker.Dequeue(out var msg, out var timestamp))
            {
                ProcessCanMessage(msg, timestamp);
            }
        }

        private static void ProcessCanMessage(PcanMessage msg, ulong timestamp)
        {
            var sender = msg.ID;

            // Commands: start logging, stop logging, transmit
            if (false)  // Check if incoming command
            {

            }
            else
            {
                var data = msg.Data;

                canBytes[0] = data[0];
                canBytes[1] = data[1];
                canBytes[2] = data[2];
                canBytes[3] = data[3];
                canBytes[4] = data[4];
                canBytes[5] = data[5];
                canBytes[6] = data[6];
                canBytes[7] = data[7];
                    

                // RPM
                // Engine Temp
                // Air Temp
                // Oil Press
                // Fuel Press
                // TPS
                // Speed
                // Lambda 
                // Lambda Target
                // DTC



                // append to file stream on sdcard
            }
        }
    }
}

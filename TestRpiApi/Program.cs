using Peak.Can.Basic;

namespace TestRpiApi
{
    public class Program
    {
        static Worker canWorker = new Worker()
        {
            TransmissionProtocol = Protocol.Can,
            BitrateCan = Bitrate.Pcan1000,
            Channel = PcanChannel.Usb01,
            AllowEchoFrames = false,
            ListenOnly = true,
        };

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
            TestWorkers();
            InitPcan();

            app.Run();
        }

        private static void TestWorkers()
        {
            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb01,
                    AllowEchoFrames = false,
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 1 pass");
            }
            catch (PcanBasicException e)
            {
                Console.WriteLine($"handle point 2: {e.Data}");
                Console.WriteLine($"handle point 2: {e.Error}");
                Console.WriteLine($"handle point 2: {e.Message}");
                Console.WriteLine($"handle point 2: {e.ApiFunction}");
                Console.WriteLine($"handle point 2: {e.InnerException}");
                Console.WriteLine($"handle point 2: {e.HelpLink}");

                Console.WriteLine("Worker 1 fail");
            }


            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb02
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 2 pass");
            }
            catch (PcanBasicException e)
            {
                Console.WriteLine($"worker 2: {e.Data}");
                Console.WriteLine($"handle point 2: {e.Error}");
                Console.WriteLine($"handle point 2: {e.Message}");
                Console.WriteLine($"handle point 2: {e.ApiFunction}");
                Console.WriteLine($"handle point 2: {e.InnerException}");
                Console.WriteLine($"handle point 2: {e.HelpLink}");
                Console.WriteLine("Worker 2 fail");
            }

            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb03
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 3 pass");
            }
            catch
            {
                Console.WriteLine("Worker 3 fail");
            }

            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb04
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 4 pass");
            }
            catch
            {
                Console.WriteLine("Worker 4 fail");
            }

            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb05
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 5 pass");
            }
            catch
            {
                Console.WriteLine("Worker 5 fail");
            }

            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb06
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 6 pass");
            }
            catch
            {
                Console.WriteLine("Worker 6 fail");
            }

            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb07
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 7 pass");
            }
            catch
            {
                Console.WriteLine("Worker 7 fail");
            }

            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb08
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 8 pass");
            }
            catch
            {
                Console.WriteLine("Worker 8 fail");
            }

            try
            {
                var w1 = new Worker()
                {
                    BitrateCan = Bitrate.Pcan1000,
                    Channel = PcanChannel.Usb09
                };

                w1.MessageAvailable += OnCanMessageAvailable;
                w1.Start();

                Console.WriteLine("Worker 9 pass");
            }
            catch
            {
                Console.WriteLine("Worker 9 fail");
            }


        }


        private static void InitPcan()
        {
            try
            {
                // PcanStatus result = Api.Initialize(PcanChannel.Usb01, Bitrate.Pcan1000);

                //Console.WriteLine("CAN Init Result: " + result.ToString());

                canWorker.MessageAvailable += OnCanMessageAvailable;
                //     canWorker.AddFilter(new FilteringCriterion()
                //   {
                //     // TODO Add filters
                //});
                canWorker.Start(false, false, true);
                Console.WriteLine("handle point 1");
            }
            catch (PcanBasicException e)
            {
                Console.WriteLine($"handle point 2: {e.Data}");
                Console.WriteLine($"handle point 2: {e.Error}");
                Console.WriteLine($"handle point 2: {e.Message}");
                Console.WriteLine($"handle point 2: {e.ApiFunction}");
                Console.WriteLine($"handle point 2: {e.InnerException}");
                Console.WriteLine($"handle point 2: {e.HelpLink}");
                throw;
            }
            catch (Exception)
            {
                Console.WriteLine("handle point 3");
                throw;
            }
        }

        public static byte[] GetBytes()
        {
            return canBytes;
        }

        private static void OnCanMessageAvailable(object? sender, MessageAvailableEventArgs e)
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

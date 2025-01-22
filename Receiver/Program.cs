using Peak.Can;
using Peak.Can.Basic;

namespace Receiver
{
    public class Program
    {
        static Worker canWorker = new Worker();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();



            app.Run();
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

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MtnDogComms;
using MtnDogShared;


namespace MtnDogLogger.Controllers
{
    public class CommsController : ControllerBase
    {
        private bool doSending = false;
        private List<string> glogList = new List<string>();

        public CommsController()
        {
            SendData();
        }

        [HttpGet]
        [Route("/test/{message}")]
        public string GetApiTest(string message)
        {
            return $"Success: {message}";
        }

        [HttpPost]
        [Route("/file")]
        public async Task PostSendFile([FromBody]List<string> logList, string targetIp = "44.0.0.2")
        {
            glogList = logList;

            var client = new MtnDogNetworkClient();
            var handshakeResult = true;// await client.PerformHandshake(DateTime.Now, 1, targetIp);

            if (handshakeResult != null)
            {
                Console.WriteLine("Handshake sucess");
              //  await client.SendLogProcessorAsync(logList, targetIp);
              doSending = true;
            }
            else
            {
                Console.WriteLine("Failed to handshake");
            }

        }

        private async void SendData(string targetIp = "44.0.0.2")
        {
            var client = new MtnDogNetworkClient();

            do
            {
                if (doSending)
                {
                    var sw = Stopwatch.StartNew();
                    Console.WriteLine("starting send");

                    await client.SendLogCompressedProcessorAsync(glogList, targetIp);

                    sw.Stop();
                    Console.WriteLine($"Total tx time: {sw.Elapsed}");

                    doSending = false;
                }
                await Task.Delay(1000);
            } while (true);

        }

        [HttpPost]
        public HandshakeResponse PostHandshakeRequest([FromBody]HandshakeRequest request)
        {
            return new HandshakeResponse()
            {
                Prefix = request.Prefix,
                TeamName = request.TeamName,
                StartTime = request.StartTime,
                PacketCount = request.PacketCount
            };
        }

        [HttpPost]
        [Route("/log")]
        public async Task PostLogMessage([FromBody]List<string> encodedLogMessage)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Incoming Log Message");

           // if (String.IsNullOrEmpty(encodedLogMessage))
           if (encodedLogMessage == null)
            {
                Console.WriteLine("Null log file");
                return;
            }

            //var messageList = JsonSerializer.Deserialize<List<string>>(encodedLogMessage);
         //   var messageList = encodedLogMessage.Split(';').ToList();

            using (var logFile = new StreamWriter("mylog.txt", true))
            {
                foreach (var logMessage in encodedLogMessage)
                {
                    await logFile.WriteAsync(logMessage);
                }

                Console.WriteLine("Wrote to file 'mylog.txt'");
            }

            sw.Stop();
            Console.WriteLine($"Total rx time: {sw.Elapsed}");
        }

        [HttpPost]
        [Route("/logcompressed")]
        public async Task PostLogMessageCompressed([FromBody] GZipStream zippedStream)
        {
            var sw = Stopwatch.StartNew();
            Console.WriteLine("Incoming Compressed Log Message");

            // if (String.IsNullOrEmpty(encodedLogMessage))
            if (zippedStream == null)
            {
                Console.WriteLine("Null stream");
                return;
            }

            
            using (var ms = new MemoryStream())
            {
                using (var unzipped = new GZipStream(zippedStream, CompressionMode.Decompress))
                {
                    unzipped.CopyTo(ms);
                    var logList = Encoding.UTF8.GetString(ms.ToArray());

                    using (var logFile = new StreamWriter("mylog.unzipped.txt", true))
                    {
                        foreach (var logMessage in logList)
                        {
                            await logFile.WriteAsync(logMessage);
                        }

                        Console.WriteLine("Wrote to file 'mylog.txt'");
                    }
                }
            }

            //var messageList = JsonSerializer.Deserialize<List<string>>(encodedLogMessage);
            //   var messageList = encodedLogMessage.Split(';').ToList();



            sw.Stop();
            Console.WriteLine($"Total rx time: {sw.Elapsed}");
        }
    }
}

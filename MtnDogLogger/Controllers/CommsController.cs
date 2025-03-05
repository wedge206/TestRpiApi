using System;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MtnDogComms;
using MtnDogShared;


namespace MtnDogLogger.Controllers
{
    public class CommsController : ControllerBase
    {
        [HttpGet]
        [Route("/test/{message}")]
        public string GetApiTest(string message)
        {
            return $"Success: {message}";
        }

        [HttpPost]
        [Route("/file")]
        public async Task PostSendFile([FromBody]string file, string targetIp = "44.0.0.2")
        {
            var logList = file.Split(';').ToList();

            var client = new MtnDogNetworkClient();
            var handshakeResult = true;// await client.PerformHandshake(DateTime.Now, 1, targetIp);

            if (handshakeResult != null)
            {
                Console.WriteLine("Handshake sucess");
                await client.SendLogProcessorAsync(logList, targetIp);
            }
            else
            {
                Console.WriteLine("Failed to handshake");
            }
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
        }
    }
}

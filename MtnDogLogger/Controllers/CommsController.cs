using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using MtnDogShared;

namespace MtnDogLogger.Controllers
{
    public class CommsController : ControllerBase
    {
        [HttpPost]
        public HandshakeResponse PostHandshakeRequest(HandshakeRequest request)
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
        public async Task PostLogMessage(string encodedLogMessage)
        {
            var messageList = encodedLogMessage.Split(';').ToList();

            using (var logFile = new StreamWriter("mylog.txt", true))
            {
                foreach (var logMessage in messageList)
                {
                    await logFile.WriteAsync(logMessage);
                }
            }
        }
    }
}

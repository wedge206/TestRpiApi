﻿using System;
using System.IO;
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
        public async Task PostSendFile([FromBody]string file)
        {
            var logList = file.Split(';').ToList();

            var client = new MtnDogNetworkClient();
            var handshakeResult = await client.PerformHandshake(DateTime.Now, 1);

            if (handshakeResult != null)
            {
                Console.WriteLine("Handshake sucess");
                await client.SendLogProcessorAsync(logList);
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
        public async Task PostLogMessage([FromBody]string encodedLogMessage)
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

using MtnDogLogger;
using MtnDogShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;

namespace MtnDogComms
{
    internal class MtnDogNetworkClient 
    {
        public async Task<HandshakeRequest> PerformHandshake(DateTime startTime, int packetCount, string targetIp)
        {
            // Using TCP/IP, we just do a simple ping test as the handshake
            using (var ping = new Ping())
            {
                var pingCount = 0;

                while (pingCount++ < 4)
                {
                    var response = await ping.SendPingAsync(targetIp, 10000);

                    Console.WriteLine($"Ping result: {response.Status}");

                    if (response.Status != IPStatus.Success)
                    {
                        return null;  // All pings must pass to be a sucessful test
                    }

                    await Task.Delay(1500);
                }

                return new HandshakeRequest()
                {
                    StartTime = startTime,
                    PacketCount = packetCount
                };
            }

            return null;
        }

        public HandshakeResponse SendHandshakeResponse(HandshakeRequest request)
        {
            // Return default result.  Handshake not needed for TCP/IP
            return new HandshakeResponse()
            {
                Prefix = request.Prefix,
                TeamName = request.TeamName,
                StartTime = request.StartTime,
                PacketCount= request.PacketCount
            };
        }

        public async Task SendLogProcessorAsync(List<string> logMessageList, string targetIp)
        {
            Console.WriteLine("Sending file");
            var encodedLog = String.Join(';', logMessageList);

            var http = new HttpClient();
            http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/text");
            var response = await http.PostAsync($"http://{targetIp}/log", new StringContent(encodedLog));

            
            Console.WriteLine($"File sent.  status: {response.StatusCode}");
        }

        public async Task SendLogProcessorAsync()
        {
            var logManager = new LogManager();
            var logList = logManager.GetLogList();

            var encodedLog = String.Join(';', logList);

            var http = new HttpClient();
            await http.PostAsync("http://44.0.0.2/log", new StringContent(encodedLog));
        }
    }
}

using MtnDogLogger;
using MtnDogShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

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

                    await Task.Delay(500);
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
                PacketCount = request.PacketCount
            };
        }

        public async Task SendLogProcessorAsync(List<string> logMessageList, string targetIp)
        {
            Console.WriteLine("Sending file");
            var sw = Stopwatch.StartNew();

            var json = JsonSerializer.Serialize(logMessageList);

            var encodedLog = String.Join(';', logMessageList);

            var http = new HttpClient();
            http.Timeout = TimeSpan.FromMinutes(10);
            var response = await http.PostAsync($"http://{targetIp}/log", new StringContent(json, Encoding.UTF8, "application/json"));

            sw.Stop();
            Console.WriteLine($"File sent.  status: {response.StatusCode}");
            Console.WriteLine($"Tx time: {sw.Elapsed}");
        }

        public async Task SendLogStreamProcessorAsync(List<string> logMessageList, string targetIp)
        {
            Console.WriteLine("Sending log stream");
            var sw = Stopwatch.StartNew();

            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, logMessageList);
                stream.Seek(0, SeekOrigin.Begin);

                using (var content = new StreamContent(stream))
                using (var client = new HttpClient())
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var response = await client.PostAsync($"http://{targetIp}/stream", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Tx Complete");
                    }
                    else
                    {
                        Console.WriteLine("Tx Failed");
                    }
                }
            }

            sw.Stop();
            Console.WriteLine($"Total Tx time: {sw.Elapsed}");
        }

        public async Task SendLogCompressedProcessorAsync(List<string> logMessageList, string targetIp)
        {
            Console.WriteLine("Sending file");
            var sw = Stopwatch.StartNew();

            var json = JsonSerializer.Serialize(logMessageList);

            var http = new HttpClient();
            http.Timeout = TimeSpan.FromMinutes(10);

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                var message = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5100");
                message.Content = new StreamContent(stream);
                var response = await http.SendAsync(message);
                //var response = await http.PostAsync($"http://{targetIp}/logcompressed", new StreamContent(zipped));

                Console.WriteLine($"File sent.  status: {response.StatusCode}");
                Console.WriteLine($"Tx time: {sw.Elapsed}");
            }
        }
        /// <summary>
        /// }
        /// </summary>
        /// <returns></returns>

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

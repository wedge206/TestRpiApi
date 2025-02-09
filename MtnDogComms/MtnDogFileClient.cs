using System;
using System.IO;
using MtnDogShared;

namespace MtnDogComms
{
    public class MtnDogFileClient : CommsClient, ICommsClient
    {
        private readonly string FilePath;

        public MtnDogFileClient(string teamName, string filePath, string senderCall, string recieverCall) : base(teamName, senderCall, recieverCall)
        {
            FilePath = filePath;
        }

        public override async Task<HandshakeRequest> PerformHandshake(DateTime startTime, int packetCount)
        {
            StopListening();

            var req = new HandshakeRequest()
            {
                TeamName = TeamName,
                StartTime = startTime,
                PacketCount = packetCount
            };

            var handshake = BuildRequest(req.EncodeMessage());
            await File.WriteAllLinesAsync(FilePath + "\\tx\\handshake.txt", new List<string>() { handshake });

            // Listen for response
            var found = false;
            do
            {
                // TODO: Implement timeout

                foreach(var fileName in Directory.EnumerateFiles(FilePath + "\\rx"))
                {
                    var file = await File.ReadAllLinesAsync(fileName);
                    var response = CheckHandshakeResponse(file.ToList());

                    if (ValidateHandshakeResponse(req, response))
                    {
                        found = true;
                    }

                    File.Delete(fileName);  // Clear file after reading
                }

                await Task.Delay(1000);
            } while (!found);

            return await Task.FromResult(req);
        }

        private HandshakeResponse CheckHandshakeResponse(List<string> file)
        {
            foreach(var line in file)
            {
                var response = ParseHandshakeResponse(line);

                if (response != null)
                {
                    return response;
                }
            }

            return null;
        }

        private HandshakeResponse ParseHandshakeResponse(string fullMessage)
        {
            var message = fullMessage.Split(':').LastOrDefault();

            return HandshakeResponse.Decode(message);
        }

        private string BuildRequest(string req)
        {
            return $"{SenderCallSign.ToUpper()}>{RecieverCallSign.ToUpper()}:{req}";
        }

        public override HandshakeResponse SendHandshakeResponse(HandshakeRequest request)
        {
            throw new NotImplementedException();
        }

        public override async Task SendLogProcessorAsync()
        {
            throw new NotImplementedException();
        }
    }
}

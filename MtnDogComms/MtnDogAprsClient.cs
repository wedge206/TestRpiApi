using System.Text;
using AprsSharp.KissTnc;
using AprsSharp.Shared;
using MtnDogLogger;
using MtnDogShared;

namespace MtnDogComms
{
    public class MtnDogAprsClient : CommsClient, ICommsClient
    {
        private TcpTnc Tnc;

        private Queue<IReadOnlyList<byte>> decodedFrames = new Queue<IReadOnlyList<byte>>();

        public MtnDogAprsClient(string teamName, string senderCall, string recieverCall, ITcpConnection direwolf) : base(teamName, senderCall, recieverCall)
        {
            direwolf.Connect("192.168.0.194", 8001);

            Tnc = new TcpTnc(direwolf, 0);
        }

        public void StartListener()
        {
            doListening = true;

            if (!isListening)
            {
                Tnc.FrameReceivedEvent += (sender, arg) => decodedFrames.Enqueue(arg.Data);

                HandshakeListener().ConfigureAwait(false);
            }
        }

        public TcpTnc GetTnc()
        {
            return Tnc;
        }

        public void StopListener()
        {
            doListening = false;
        }

        public async Task HandshakeListener()
        {
            while (doListening)
            {
                isListening = true;

                while (decodedFrames.TryDequeue(out var frame))
                {
                    var message = Encoding.ASCII.GetString(frame.ToArray());

                    if (message.Substring(0, 8) == HandshakeRequest.prefix.ToString())
                    {
                        SendHandshakeResponse(HandshakeRequest.Decode(message));
                    }
                }

                await Task.Delay(5000);
            }

            Tnc.FrameReceivedEvent -= (sender, arg) => decodedFrames.Enqueue(arg.Data);
            decodedFrames.Clear();

            isListening = false;
        }

        public override async Task SendLogProcessorAsync()
        {
            var logManager = new LogManager();
            var logList = logManager.GetLogList();

            foreach (var logName in logList)
            {
                var logFile = LogFile.GetLog(logName);
                var packetCount = logFile.GetLineCount();
                var handshake = PerformHandshake(DateTime.Now, packetCount);

                if (handshake == null)
                {
                    // Failed handshake
                    break;  //TODO: manage this better
                }

                // TODO: Startup retry listener
                await foreach(var line in logFile.ReadLogLinesAsync())
                {
                    Tnc.SendData(Encoding.ASCII.GetBytes(line.ToArray()));
                }

                await Task.Delay(1000);  // Slight delay before ending

                var completedMessage = new LogTxComplete(TeamName, packetCount);
                Tnc.SendData(Encoding.ASCII.GetBytes(completedMessage.Encode()));

                //  TODO: Check for retry requests
                //     Resend requested packets
            }
        }

        public override Task<HandshakeRequest> PerformHandshake(DateTime startTime, int packetCount)
        {
            StopListening();

            var req = new HandshakeRequest()
            {
                TeamName = TeamName,
                StartTime = startTime,
                PacketCount = packetCount
            };

            Tnc.SendData(Encoding.ASCII.GetBytes(req.EncodeMessage().ToArray()));

            // Start listening for response
            Tnc.FrameReceivedEvent += (sender, arg) => decodedFrames.Enqueue(arg.Data);

            var awaitingResponse = true;
            var startedHandshake = DateTime.Now;

            while (awaitingResponse)
            {
                if (decodedFrames.TryDequeue(out var frame))
                {
                    if (ResponseProcessor(Encoding.ASCII.GetString(frame.ToArray()), req))
                    {
                        awaitingResponse = false;
                    }
                }

                if (startedHandshake.AddSeconds(60) < DateTime.Now)
                {
                    return null;  // Failed Handshake
                }
            }

            Tnc.FrameReceivedEvent -= (sender, arg) => decodedFrames.Enqueue(arg.Data);
            decodedFrames.Clear();

            return Task.FromResult(req); // Successful handshake
        }

        public override HandshakeResponse SendHandshakeResponse(HandshakeRequest request)
        {
            if (request.TeamName != TeamName)
            {
                return null;  // Not our message
            }

            var response = new HandshakeResponse()
            {
                TeamName = TeamName,
                StartTime = request.StartTime,
                PacketCount = request.PacketCount
            };

            Tnc.SendData(Encoding.ASCII.GetBytes(response.EncodeMessage().ToArray()));

            return response;
        }

        private bool ResponseProcessor(string message, HandshakeRequest request)
        {
            var decoded = HandshakeResponse.Decode(message);

            return ValidateHandshakeResponse(request, decoded);
        }
    }
}

using MtnDogShared;
using System.Text;

namespace MtnDogComms
{
    public abstract class CommsClient(string teamName, string senderCallSign, string recieverCallSign) : ICommsClient
    {
        protected bool isListening = false;

        protected bool doListening = false;

        public readonly string TeamName = teamName;

        public readonly string SenderCallSign = senderCallSign;

        public readonly string RecieverCallSign = recieverCallSign;

        public abstract Task<HandshakeRequest> PerformHandshake(DateTime startTime, int packetCount);

        public abstract HandshakeResponse SendHandshakeResponse(HandshakeRequest request);

        public abstract Task SendLogProcessorAsync();

        protected bool ValidateHandshakeResponse(HandshakeRequest request, HandshakeResponse response)
        {
            if (request == null || response == null)
            {
                return false;
            }

            if (response.TeamName == request.TeamName && response.StartTime == request.StartTime && response.PacketCount == request.PacketCount)
            {
                return true;
            }

            return false;
        }

        protected void StopListening()
        {
            doListening = false;

            while (isListening)
            {
                // Wait for any previous listener to stop
            }
        }
    }
}

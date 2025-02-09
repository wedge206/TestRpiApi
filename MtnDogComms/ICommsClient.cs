using MtnDogShared;

namespace MtnDogComms
{
    internal interface ICommsClient
    {
        public Task SendLogProcessorAsync();

        public Task<HandshakeRequest> PerformHandshake(DateTime startTime, int packetCount);

        public HandshakeResponse SendHandshakeResponse(HandshakeRequest request);
    }
}

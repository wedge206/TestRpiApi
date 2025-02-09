namespace MtnDogShared
{
    public interface IHandshakeMessage
    {
        public string TeamName { get; set; }

        public DateTime StartTime { get; set; }

        public int PacketCount { get; set; }

        public string EncodeMessage();
    }
}

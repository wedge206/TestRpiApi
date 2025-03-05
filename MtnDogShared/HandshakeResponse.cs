namespace MtnDogShared
{
    public class HandshakeResponse : HandshakeMessage<HandshakeResponse>
    {
        public HandshakeResponse() : base()
        {
        }

        public static HandshakeResponse Decode(string message)
        {
            throw new NotImplementedException();
        }
    }
}

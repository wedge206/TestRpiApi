namespace MtnDogShared
{
    public class HandshakeResponse : HandshakeMessage<HandshakeResponse>
    {
        public const string prefix = "02020202";
        
        public const string suffix = "8080808";

        public HandshakeResponse() : base(prefix, suffix)
        {
        }

        public static HandshakeResponse Decode(string message)
        {
            throw new NotImplementedException();
        }
    }
}

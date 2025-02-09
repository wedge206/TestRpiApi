namespace MtnDogShared
{
    public class HandshakeRequest : HandshakeMessage<HandshakeRequest>
    {
        public const string prefix = "08080808";

        public const string suffix = "02020202";

        public HandshakeRequest() : base(prefix, suffix)
        {
        }
    }
}

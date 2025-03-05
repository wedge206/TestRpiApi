namespace MtnDogShared
{
    public class LogRetryRequest : IMessage<LogRetryRequest>
    {
        private const string prefix = "QSM";

        public string Prefix { get; set; } = prefix;

        public LogRetryRequest DecodeMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}

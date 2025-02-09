namespace MtnDogShared
{
    public class LogTxComplete : IMessage<LogTxComplete>
    {
        private const string prefix = "09090909";

        private const string suffix = "01010101";

        private const int value = 0;

        public string Prefix { get; set; } = prefix;

        public string Suffix { get; set; } = suffix;

        public int Value { get; set; } = value;

        public string TeamName { get; set; }

        public int PacketCount { get; set; }

        public LogTxComplete(string teamName, int packetCount)
        {
            TeamName = teamName;
            PacketCount = packetCount;
        }

        public LogTxComplete DecodeMessage(string message)
        {
            return Decode(message);
        }

        public string Encode()
        {
            return $"{Prefix}{Value.ToString("D8")}{String.Format("{0, -8}", TeamName)}{PacketCount.ToString("D8")}{Suffix}";
        }

        public static LogTxComplete Decode(string message)
        {
            try
            {
                var pre = message.Substring(0, 8);
                var val = int.Parse(message.Substring(8, 8));
                var team = message.Substring(16, 8);
                var num = int.Parse(message.Substring(24, 8));
                var suf = message.Substring(32, 8);

                if (pre == prefix && val == value && suf == suffix)
                {
                    return new LogTxComplete(team, num);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public override bool Equals(object? obj)
        {
            var val = obj as LogTxComplete;

            if (val == null)
            {
                return false;
            }

            return Prefix.Equals(val.Prefix) && Value.Equals(val.Value) && Suffix.Equals(val.Suffix);
        }

        public override int GetHashCode()
        {
            return Prefix.GetHashCode() ^ Value.GetHashCode() ^ Suffix.GetHashCode();
        }
    }
}

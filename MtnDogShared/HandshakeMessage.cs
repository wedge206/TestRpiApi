namespace MtnDogShared
{
    public abstract class HandshakeMessage<T> : IHandshakeMessage, IMessage<T> where T : IHandshakeMessage, new()
    {
        public const string teamName = "MtnDogRally";

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public string TeamName { get; set; } = teamName;

        public DateTime StartTime { get; set; }

        public int PacketCount { get; set; }

        public HandshakeMessage(string prefix, string suffix)
        {
            Prefix = prefix;
            Suffix = suffix;
        }

        public string EncodeMessage()
        {
            return Prefix
                + TeamName
                + EncodeDatetime(StartTime)
                + EncodePacketCount(PacketCount)
                + Suffix;
        }

        private string EncodeDatetime(DateTime dt)
        {
            return dt.ToString("yyMMddHHmmss");
        }

        private string EncodePacketCount(int packetCount)
        {
            return packetCount.ToString("D5");
        }

        private static DateTime DecodeDatetime(string message, int position)
        {
            var start = message.AsSpan(position, 2);
            if (int.TryParse(message.AsSpan(position, 2), out var year)
                && int.TryParse(message.AsSpan(position + 2, 2), out var month)
                && int.TryParse(message.AsSpan(position + 4, 2), out var day)
                && int.TryParse(message.AsSpan(position + 6, 2), out var hour)
                && int.TryParse(message.AsSpan(position + 8, 2), out var minute)
                && int.TryParse(message.AsSpan(position + 10, 2), out var second))
            {
                // Do some basic validation
                if (year < 0 || year > 50)
                {
                    return DateTime.MinValue;
                }
                else if (month < 1 || month > 12)
                {
                    return DateTime.MinValue;
                }
                else if (day < 1 || day > 31)
                {
                    return DateTime.MinValue;
                }
                else if (hour < 0 || hour > 24)
                {
                    return DateTime.MinValue;
                }
                else if (minute < 0 || minute > 60)
                {
                    return DateTime.MinValue;
                }
                else if (second < 0 || second > 60)
                {
                    return DateTime.MinValue;
                }

                return new DateTime(year + 2000, month, day, hour, minute, second);
            }

            return DateTime.MinValue;
        }

        public T DecodeMessage(string message)
        {
            return Decode(message);
        }

        public static T Decode(string message)
        {
            var expectedLength = 33 + teamName.Length;
            if (message.Length != expectedLength)
            {
                return default;
                //throw new Exception($"Invalid TxMessage.  Message length should be {expectedLength}.  Actual length is {message.Length}");
            }

            var messagePrefix = message.AsSpan(0, 8).ToString();

            if (messagePrefix == "08080808")
            {
                return new T()
                {
                    StartTime = DecodeDatetime(message, 8 + teamName.Length),
                    PacketCount = DecodePacketCount(message, 20 + teamName.Length)
                };
            }
            else if (messagePrefix == "02020202")
            {
                return new T()
                {
                    StartTime = DecodeDatetime(message, 8 + teamName.Length),
                    PacketCount = DecodePacketCount(message, 20 + teamName.Length)
                };
            }

            return default;
        }

        private static int DecodePacketCount(string message, int position)
        {
            if (int.TryParse(message.AsSpan(position, 5), out var count))
            {
                return count;
            }

            return -1;
        }

        public bool IsHandshakeMessage(string message)
        {
            if (message.Length != 28 + TeamName.Length)
            {
                return false;
            }

            if (message.StartsWith(Prefix.ToString()) && message.EndsWith(Suffix.ToString()))
            {
                return true;
            }

            return false;
        }
    }
}

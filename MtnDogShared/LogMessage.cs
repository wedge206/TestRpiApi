namespace MtnDogShared
{
    public class LogMessage : IMessage<LogMessage>, IFormattable
    {
        private const int length = 43;

        // Seconds
        public decimal TimeStamp { get; set; }

        public int RPM { get; set; }

        public int EngineTemp { get; set; }

        public int AirTemp { get; set; }

        public int OilTemp { get; set; }

        public int FuelTemp { get; set; }

        public int OilPress {  get; set; }

        public int FuelPress { get; set; }

        public int TPS { get; set; }

        public decimal Lambda { get; set; }

        public decimal LambdaTarget { get; set; }

        public int Volts { get; set; }

        public int Gear { get; set; }

        public int FuelLevel { get; set; }

        public int VehicleSpeed { get; set; }

        public int PacketNumber { get; set; }

        public string Prefix { get; set; } = "";

        public string Suffix { get; set; } = "";

        public string EncodeMessage()
        {
            return EncodeTimeStamp(TimeStamp)
                + EncodeRpm(RPM)
                + EncodeTemp(EngineTemp)
                + EncodeTemp(AirTemp)
                + EncodeTemp(OilTemp)
                + EncodeTemp(FuelTemp)
                + EncodePress(OilPress)
                + EncodePress(FuelPress)
                + EncodePercent(TPS)
                + EncodeLambda(Lambda)
                + EncodeLambda(LambdaTarget)
                + EncodeVolts(Volts)
                + EncodeGear(Gear)
                + EncodePercent(FuelLevel)
                + EncodeSpeed(VehicleSpeed)
                + EncodePacketNum(PacketNumber);
        }

        public string ToCsv()
        {
            return $"{TimeStamp},{RPM},{EngineTemp},{AirTemp},{OilTemp},{FuelTemp},{FuelPress},{TPS},{Lambda},{LambdaTarget},{Volts},{Gear},{FuelLevel},{VehicleSpeed}";
        }

        public static LogMessage ParseCsv(string csvLine, int lineNum)
        {
            var logData = csvLine.Split(',');

            return  new LogMessage()
            {
                TimeStamp = csvLine[0],
                RPM = csvLine[1],
                EngineTemp = csvLine[2],
                AirTemp = csvLine[3],
                OilTemp = csvLine[4],
                FuelTemp = csvLine[5],
                FuelPress = csvLine[6],
                TPS = csvLine[7],
                Lambda = csvLine[8],
                LambdaTarget = csvLine[9],
                Volts = csvLine[10],
                Gear = csvLine[11],
                FuelLevel = csvLine[12],
                VehicleSpeed = csvLine[13],
                PacketNumber = lineNum
            };
        }

        public LogMessage DecodeMessage(string message)
        {
            return Decode(message);
        }

        public static LogMessage Decode(string message)
        {
            if (message.Length != length)
            {
                throw new Exception($"Invalid TxMessage.  Message length should be {length}.  Actual length is {message.Length}");
            }

            return new LogMessage()
            {
                TimeStamp = DecodeTimeStamp(message),
                RPM = DecodeRpm(message),
                EngineTemp = DecodeTemp(message, 8),
                AirTemp = DecodeTemp(message, 11),
                OilTemp = DecodeTemp(message, 14),
                FuelTemp = DecodeTemp(message, 17),
                OilPress = DecodePress(message, 20),
                FuelPress = DecodePress(message, 22),
                TPS = DecodePercent(message, 24),
                Lambda = DecodeLambda(message, 26),
                LambdaTarget = DecodeLambda(message, 28),
                Volts = DecodeVolts(message, 30),
                Gear = DecodeGear(message, 32),
                FuelLevel = DecodePercent(message, 33),
                VehicleSpeed = DecodeSpeed(message, 35),
                PacketNumber = DecodePacketNum(message, 38)
            };
        }

        public static bool IsLogMessage(string message)
        {
            if (message.Length != length)
            {
                return false;
            }

            if (DecodeTimeStamp(message) < 0)
            {
                return false;
            }

            if (DecodePacketNum(message, 38) < 0)
            {
                return false;
            }

            return true;
        }

        private string EncodePress(int press)
        {
            if (press < 0 )
            {
                return "00";
            }
            else if (press >= 100)
            {
                return "99";
            }

            return press.ToString("D2");
        }

        private static int DecodePress(string message, int position)
        {
            if (int.TryParse(message.AsSpan(position, 2), out var press))
            {
                return press;
            }

            return 0;
        }

        private string EncodeRpm(int rpm)
        {
            return (rpm / 10).ToString("D3");
        }

        private static int DecodeRpm(string message)
        {
            if (int.TryParse(message.AsSpan(5, 3), out var rpm))
            {
                return rpm * 10;
            }

            return 0;
        }

        private string EncodeTimeStamp(decimal timestamp)
        {
            return ((int)(timestamp * 10)).ToString("D5");
        }

        private string EncodePacketNum(int num)
        {
            return num.ToString("D5");
        }

        private static decimal DecodeTimeStamp(string message)
        {
            if (decimal.TryParse(message.AsSpan(0, 5), out var time))
            {
                return (time / 10);
            }

            return -1;
        }

        private static int DecodePacketNum(string message, int position)
        {
            if (int.TryParse(message.AsSpan(position, 5), out var num))
            {
                return num;
            }

            return -1;
        }

        private string EncodeGear(int gear)
        {
            switch (gear)
            {
                case -1: return "R";
                case 0: return "N";
                default: return gear.ToString();
            }
        }

        private static int DecodeGear(string message, int position)
        {
            var gearString = message.AsSpan(position, 1);

            if (gearString == "R")
            {
                return -1;
            }
            else if (gearString == "N")
            {
                return 0;
            }
            else if (int.TryParse(gearString, out var gear))
            {
                return gear;
            }

            return 0;
        }

        private string EncodeVolts(int volts)
        {
            return volts.ToString("D2");
        }

        private static int DecodeVolts(string message, int position)
        {
            if (int.TryParse(message.AsSpan(position, 2), out var volts))
            {
                return volts;
            }

            return 0;
        }

        private string EncodeSpeed(int speed)
        {
            return speed.ToString("D3");
        }

        private static int DecodeSpeed(string message, int position)
        {
            if (int.TryParse(message.AsSpan(position, 3), out var speed))
            {
                return speed;
            }

            return 0;
        }

        private string EncodeLambda(decimal lambda)
        {
            if (lambda <= 0.5m)
            {
                return "51";
            }
            if (lambda >= 1.5m)
            {
                return "50";
            }
            if (lambda > 1)
            {
                return ((int)((lambda - 1) * 100)).ToString("D2");
            }
            else
            {
                return ((int)(lambda * 100)).ToString("D2");
            }
        }

        private static decimal DecodeLambda(string message, int position)
        {
            if (decimal.TryParse(message.AsSpan(position, 2), out var lambda))
            {
                if (lambda <= 50)
                {
                    var val = (lambda / 100) + 1;
                    return val;
                }
                else
                {
                    var val = (lambda / 100);
                    return val;
                }
            }

            return 1;
        }

        private string EncodePercent(int percent)
        {
            if (percent > 99)
            {
                return "99";  // This causes a small amount of data loss, but reduces Tx efficiency
            }
            else
            {
                return percent.ToString("D2");
            }
        }

        private static int DecodePercent(string message, int position)
        {
            if (int.TryParse(message.AsSpan(position, 2), out var percent))
            {
                return percent;
            }

            return 0;
        }

        private string EncodeTemp(int temp)
        {
            if (temp < -99)
            {
                return "-99";
            }
            else if (temp > 999)
            {
                return "999";
            }
            else if (temp < 0)
            {
                return $"{temp.ToString("D2")}";
            }
            else
            {
                return temp.ToString("D3");
            }
        }

        private static int DecodeTemp(string message, int position)
        {
            if (int.TryParse(message.AsSpan(position, 3), out var temp))
            {
                return temp;
            }

            return 0;
        }

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return EncodeMessage();
        }
    }
}

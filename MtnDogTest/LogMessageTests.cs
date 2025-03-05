using MtnDogShared;

namespace MtnDogTest
{
    [TestClass]
    public sealed class LogMessageTests
    {
        [TestMethod]
        public void TestDecodeOnly()
        {
            var testMessage = "0005235610502510004651586195901355806301549";

            var decoded = LogMessage.Decode(testMessage);

            Assert.AreEqual(5.2m, decoded.TimeStamp);
            Assert.AreEqual(3560, decoded.RPM);
            Assert.AreEqual(105, decoded.EngineTemp);
            Assert.AreEqual(25, decoded.AirTemp);
            Assert.AreEqual(100, decoded.OilTemp);
            Assert.AreEqual(46, decoded.FuelTemp);
            Assert.AreEqual(51, decoded.OilPress);
            Assert.AreEqual(58, decoded.FuelPress);
            Assert.AreEqual(61, decoded.TPS);
            Assert.AreEqual(0.95m, decoded.Lambda);
            Assert.AreEqual(0.90m, decoded.LambdaTarget);
            Assert.AreEqual(13, decoded.Volts);
            Assert.AreEqual(5, decoded.Gear);
            Assert.AreEqual(58, decoded.FuelLevel);
            Assert.AreEqual(63, decoded.VehicleSpeed);
            Assert.AreEqual(1549, decoded.PacketNumber);
        }

        [TestMethod]
        public void TestEncodeOnly()
        {
            var timestamp = 101.9m;
            var rpm = 2584;
            var engineTemp = 89;
            var airTemp = 31;
            var oilTemp = 120;
            var fuelTemp = 12;
            var oilPress = 70;
            var fuelPress = 58;
            var tps = 54;
            var lambda = 0.92m;
            var lambdaTarget = 0.90m;
            var volts = 12;
            var gear = 1;
            var fuelLevel = 55;
            var speed = 62;
            var packetNum = 1099;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                RPM = rpm,
                EngineTemp = engineTemp,
                AirTemp = airTemp,
                OilTemp = oilTemp,
                FuelTemp = fuelTemp,
                OilPress = oilPress,
                FuelPress = fuelPress,
                TPS = tps,
                Lambda = lambda,
                LambdaTarget = lambdaTarget,
                Volts = volts,
                Gear = gear,
                FuelLevel = fuelLevel,
                VehicleSpeed = speed,
                PacketNumber = packetNum,
            };

            var encoded = LogMessage.EncodeMessage();

            Assert.AreEqual("0101925808903112001270585492901215506201099", encoded);
        }

        [TestMethod]
        public void TestPressure()
        {
            var timestamp = 101.9m;
            var oilPress = 1;
            var fuelPress = 99;
            var packetNum = 99;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                OilPress = oilPress,
                FuelPress = fuelPress,
                PacketNumber = packetNum
            };

            var encoded = LogMessage.EncodeMessage();

            var decoded = LogMessage.DecodeMessage(encoded);

            Assert.AreEqual(oilPress, decoded.OilPress);
            Assert.AreEqual(fuelPress, decoded.FuelPress);
        }

        [TestMethod]
        public void TestInvalidPressure()
        {
            var timestamp = 101.9m;
            var oilPress = -1;
            var fuelPress = 101;
            var packetNum = 99;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                OilPress = oilPress,
                FuelPress = fuelPress,
                PacketNumber = packetNum
            };

            var encoded = LogMessage.EncodeMessage();

            var decoded = LogMessage.DecodeMessage(encoded);

            Assert.AreEqual(0, decoded.OilPress);
            Assert.AreEqual(99, decoded.FuelPress);
        }

        [TestMethod]
        public void TestLambda()
        {
            var timestamp = 101.9m;
            var lambda = 0.95m;
            var targetLambda = 1.09m;
            var packetNum = 99;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                Lambda = lambda,
                LambdaTarget = targetLambda,
                PacketNumber = packetNum
            };

            var encoded = LogMessage.EncodeMessage();

            var decoded = LogMessage.DecodeMessage(encoded);

            Assert.AreEqual(lambda, decoded.Lambda);
            Assert.AreEqual(targetLambda, decoded.LambdaTarget);
        }

        [TestMethod]
        public void TestExtremeLambda()
        {
            var timestamp = 101.9m;
            var lambda = 1.50m;
            var targetLambda = 0.50m;
            var packetNum = 99;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                Lambda = lambda,
                LambdaTarget = targetLambda,
                PacketNumber = packetNum
            };

            var encoded = LogMessage.EncodeMessage();

            var decoded = LogMessage.DecodeMessage(encoded);

            Assert.AreEqual(1.5m, decoded.Lambda);
            Assert.AreEqual(0.51m, decoded.LambdaTarget);
        }

        [TestMethod]
        public void TestInvalidLambda()
        {
            var timestamp = 101.9m;
            var lambda = 1.51m;
            var targetLambda = 0.49m;
            var packetNum = 99;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                Lambda = lambda,
                LambdaTarget = targetLambda,
                PacketNumber = packetNum
            };

            var encoded = LogMessage.EncodeMessage();

            var decoded = LogMessage.DecodeMessage(encoded);

            Assert.AreEqual(1.5m, decoded.Lambda);
            Assert.AreEqual(0.51m, decoded.LambdaTarget);
        }

        [TestMethod]
        public void TestNegativeTemps()
        {
            var timestamp = 101.9m;
            var engineTemp = 0;
            var airTemp = -10;
            var oilTemp = -1;
            var fuelTemp = -99;
            var packetNum = 99;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                EngineTemp = engineTemp,
                AirTemp = airTemp,
                OilTemp = oilTemp,
                FuelTemp = fuelTemp,
                PacketNumber = packetNum
            };

            var encoded = LogMessage.EncodeMessage();

            var decoded = LogMessage.DecodeMessage(encoded);

            Assert.AreEqual(engineTemp, decoded.EngineTemp, "Bad Engine Temp");
            Assert.AreEqual(airTemp, decoded.AirTemp, "Bad Air Temp");
            Assert.AreEqual(oilTemp, decoded.OilTemp, "Bad Oil Temp");
            Assert.AreEqual(fuelTemp, decoded.FuelTemp, "Bad Fuel Temp");
        }

        [TestMethod]
        public void TestInvalidTemp()
        {
            var timestamp = 101.9m;
            var engineTemp = 1001;
            var airTemp = -100;
            var oilTemp = -999;
            var fuelTemp = 999;
            var packetNum = 99;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                EngineTemp = engineTemp,
                AirTemp = airTemp,
                OilTemp = oilTemp,
                FuelTemp = fuelTemp,
                PacketNumber = packetNum
            };

            var encoded = LogMessage.EncodeMessage();

            var decoded = LogMessage.DecodeMessage(encoded);

            Assert.AreEqual(999, decoded.EngineTemp, "Bad Engine Temp");
            Assert.AreEqual(-99, decoded.AirTemp, "Bad Air Temp");
            Assert.AreEqual(-99, decoded.OilTemp, "Bad Oil Temp");
            Assert.AreEqual(999, decoded.FuelTemp, "Bad Fuel Temp");
        }

        [TestMethod]
        public void TestFullMessageEncodeDecode()
        {
            var timestamp = 101.9m;
            var rpm = 8126;
            var engineTemp = 89;
            var airTemp = -43;
            var oilTemp = 120;
            var fuelTemp = 12;
            var oilPress = 70;
            var fuelPress = 58;
            var tps = 22;
            var lambda = 0.92m;
            var lambdaTarget = 0.90m;
            var volts = 12;
            var gear = 3;
            var fuelLevel = 55;
            var speed = 101;
            var packetNum = 99;

            var LogMessage = new LogMessage()
            {
                TimeStamp = timestamp,
                RPM = rpm,
                EngineTemp = engineTemp,
                AirTemp = airTemp,
                OilTemp = oilTemp,
                FuelTemp = fuelTemp,
                OilPress = oilPress,
                FuelPress = fuelPress,
                TPS = tps,
                Lambda = lambda,
                LambdaTarget = lambdaTarget,
                Volts = volts,
                Gear = gear,
                FuelLevel = fuelLevel,
                VehicleSpeed = speed,
                PacketNumber = packetNum,
            };

            var encoded = LogMessage.EncodeMessage();

            Assert.AreEqual("01019812089-4312001270582292901235510100099", encoded);

            var decoded = LogMessage.DecodeMessage(encoded);

            Assert.AreEqual(timestamp, decoded.TimeStamp);
            Assert.AreEqual((rpm / 10) * 10, decoded.RPM);
            Assert.AreEqual(engineTemp, decoded.EngineTemp);
            Assert.AreEqual(airTemp, decoded.AirTemp);
            Assert.AreEqual(oilTemp, decoded.OilTemp);
            Assert.AreEqual(fuelTemp, decoded.FuelTemp);
            Assert.AreEqual(oilPress, decoded.OilPress);
            Assert.AreEqual(fuelPress, decoded.FuelPress);
            Assert.AreEqual(tps, decoded.TPS);
            Assert.AreEqual(lambda, decoded.Lambda);
            Assert.AreEqual(lambdaTarget, decoded.LambdaTarget);
            Assert.AreEqual(volts, decoded.Volts);
            Assert.AreEqual(gear, decoded.Gear);
            Assert.AreEqual(fuelLevel, decoded.FuelLevel);
            Assert.AreEqual(speed, decoded.VehicleSpeed);
            Assert.AreEqual(packetNum, decoded.PacketNumber);
        }

        [TestMethod]
        public void TestLogComplete()
        {
            var team = "testTeam";
            var num = 51;

            var logComplete = new LogTxComplete(team, num);

            var testMessage = "QSL00000000testTeam00000051";

            var decodedMessage = LogTxComplete.Decode(testMessage);

            Assert.IsNotNull(decodedMessage);

            Assert.AreEqual(logComplete, decodedMessage);
            Assert.AreEqual(team, decodedMessage.TeamName);
            Assert.AreEqual(num, decodedMessage.PacketCount);
        }

        [TestMethod]
        public void TestLogCompleteDecode()
        {
            var testMessage = "1090909090000000001010101";  // Intentionally bad message

            var decodedMessage = LogTxComplete.Decode(testMessage);

            Assert.IsNull(decodedMessage);
        }


        [TestMethod]
        public void TestLogCompleteEquals()
        {
            var team = "testTeam";
            var num = 999;

            var logComplete = new LogTxComplete(team, num);

            Assert.IsTrue(logComplete.Equals(logComplete));

            object obj = null;

            Assert.IsFalse(logComplete.Equals(obj));

            obj = "";

            Assert.IsFalse(logComplete.Equals(obj));
        }
    }
}

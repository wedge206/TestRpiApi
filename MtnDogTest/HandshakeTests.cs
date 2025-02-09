using MtnDogShared;

namespace MtnDogTest
{
    [TestClass]
    public sealed class HandshakeTests
    {
        [TestMethod]
        public void TestEncodeDecodeRequest()
        {
            var request = new HandshakeRequest()
            {
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                PacketCount = 101
            };
            
            var encoded = request.EncodeMessage();

            var decoded = HandshakeRequest.Decode(encoded);

            var t1 = request.StartTime.TimeOfDay;
            var t2 = decoded.StartTime.TimeOfDay;

            Assert.AreEqual(request.Prefix, decoded.Prefix);
            Assert.AreEqual(request.Suffix, decoded.Suffix);

            Assert.AreEqual(request.StartTime.Date, decoded.StartTime.Date);
            Assert.AreEqual(request.StartTime, decoded.StartTime);

            Assert.AreEqual(request.PacketCount, decoded.PacketCount);
        }

        [TestMethod]
        public void TestNonHandshake()
        {
            var request = "invalid message";

            var decoded = HandshakeRequest.Decode(request);

            Assert.IsNull(decoded);
        }

        [TestMethod]
        public void TestInvalidHandshake()
        {
            var length = 34 + HandshakeRequest.teamName.Length;

            var request = new string('1', length);

            var decoded = HandshakeRequest.Decode(request);

            Assert.IsNull(decoded);
        }
    }
}

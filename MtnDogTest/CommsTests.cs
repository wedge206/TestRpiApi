using AprsSharp.Shared;
using Microsoft.VisualStudio.TestPlatform.Common.Hosting;

using MtnDogComms;
using MtnDogShared;

namespace MtnDogTest
{
    [TestClass]
    public sealed class CommsTests
    {
        // public ITcpConnection BuildMockConnection()
        //{
        // var mockConn = A.Fake<ITcpConnection>();

        //A.CallTo(() => mockConn.ReceiveString()).Returns(A<string>.Ignored);
        //var mockConn = new Mock<ITcpConnection>();
        //mockConn.SetupGet(mock => mock.Connected).Returns(true);
        //mockConn.Setup(mock => mock.AsyncReceive(It.IsAny<HandleReceivedBytes>()));
        //mockConn.Setup(mock => mock.SendBytes(It.IsAny<byte[]>()));
        //mockConn.Setup(mock => mock.SendString(It.IsAny<string>()));
        //
        //           return mockConn;
        //}

        [TestMethod]
        public void TestHandshakeResponseValidation()
        {
            var request = new HandshakeRequest();
            var response = new HandshakeResponse();

            var aprsClient = new MtnDogAprsClient("MtnDog", "", "", null);
            var fileClient = new MtnDogFileClient("MtnDog", "", "", "");

            //
        }

        [TestMethod]
        public void TestHandshakeListener()
        {
            //var conn = BuildMockConnection();




            //var reciever = new Comms(conn);

            //reciever.StartListener();

            //conn.SendString("testthis");


        }

        [TestMethod]
        public void TestPerformHandshake()
        {
            //var startTime = DateTime.Now;
            //var packetCount = 9901;

            //var conn = BuildMockConnection();

            //A.CallTo(() => conn.SendBytes(A<byte[]>.Ignored)).Invokes(a =>
            //{
            //    var test = true;
            //});

            ////conn.Setup(mock => mock.SendBytes(It.IsAny<byte[]>()))
            ////    .Callback(() => 
            ////    {

            ////        var test = true;
            ////    });

            ////conn.Setup(mock => mock.AsyncReceive(It.IsAny<HandleReceivedBytes>()))
            ////    .Callback<HandleReceivedBytes>(bytes => 
            ////    {
            ////        var test = true;
            ////    });



            //var sender = new Comms(conn);


            //var result = sender.PerformHandshake(startTime, packetCount);

            //Assert.IsNotNull(result);
        }
    }
}

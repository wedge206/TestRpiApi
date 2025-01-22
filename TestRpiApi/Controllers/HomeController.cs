using Microsoft.AspNetCore.Mvc;
using AprsSharp;
using AprsSharp.KissTnc;
using AprsSharp.Shared;
using AprsSharp.AprsParser;
using AprsSharp.AprsIsClient;
using System.Text;
using SocketCANSharp;
using SocketCANSharp.Network;


namespace TestRpiApi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/{value}")]
        public IActionResult Index(string value)
        {
            try
            {

                 var direwolf = new TcpConnection();
                
                    direwolf.Connect("192.168.0.194", 8001);



                    var info = new MessageInfo("ID1", "mymessage", null);
                
                    var pac = new Packet("WSEV", "DEST", new List<string>(), info);


                    var KISS = new TcpTnc(direwolf, 0x0);
                    var text = pac.EncodeTnc2();
                KISS.SendData(pac.EncodeAx25());
                   // direwolf.SendString(value);

                KISS.SendData(Encoding.ASCII.GetBytes(value));

                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<html><head></head><body>If you can read this, I don't need glasses.</body></html>",
                    StatusCode = 200
                };
            }
            catch {
                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<html><head></head><body>Crash report</body></html>",
                    StatusCode = 200
                };
            }
        }
    }
}

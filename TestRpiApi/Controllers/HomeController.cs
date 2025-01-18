using Microsoft.AspNetCore.Mvc;
using AprsSharp;
using AprsSharp.KissTnc;
using AprsSharp.Shared;

namespace TestRpiApi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            var direwolf = new TcpConnection();
            direwolf.Connect("http://localhost", 8001);

            var KISS = new TcpTnc(direwolf, 0x0);
            var text = "WSEV21>ID:Testthis";
            KISS.SendData(System.Text.Encoding.UTF8.GetBytes(text));

            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<html><head></head><body>If you can read this, I don't need glasses.</body></html>",
                StatusCode = 200
            };
        }
    }
}

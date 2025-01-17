using Microsoft.AspNetCore.Mvc;

namespace TestRpiApi.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return new ContentResult
            {
                ContentType = "text/html",
                Content = "<html><head></head><body>If you can read this, I don't need glasses.</body></html>",
                StatusCode = 200
            };
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Server
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello from the server");
        }
    }
}
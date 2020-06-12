using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Server.Controllers
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
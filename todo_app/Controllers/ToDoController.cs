using Microsoft.AspNetCore.Mvc;

namespace todo_app.Controllers
{
    public class ToDoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using todo_library.Models;

namespace todo_app.Controllers
{
    public class ToDoController : Controller
    {
        private readonly HttpClient client;

        public ToDoController()
        {
            client = todo_library.Helpers.StaticHelpers.Create();
        }

        public async Task<IActionResult> Index()
        {
            string token = Request.Cookies["Token"];

            if (string.IsNullOrWhiteSpace(token)) return RedirectToAction("Login", "Auth");

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = await client.GetAsync("todos/getAllToDos");

            if (response.IsSuccessStatusCode)
            {
                string toDosResponse = await response.Content.ReadAsStringAsync();
                List<ToDo> toDos = JsonConvert.DeserializeObject<List<ToDo>>(toDosResponse);

                return View(toDos);
            }

            return View();
        }

        public async Task<IActionResult> AddToDo([Bind("Title")] ToDo toDo)
        {
            string token = Request.Cookies["Token"];

            if (string.IsNullOrWhiteSpace(token)) return RedirectToAction("Login", "Auth");

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            HttpResponseMessage response = await client.PostAsJsonAsync("todos/addToDo?title=" + toDo.Title, toDo.Title);

            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View("Index");
        }
    }
}

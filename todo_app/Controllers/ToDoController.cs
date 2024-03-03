﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
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
            todo_library.Helpers.StaticHelpers.SetAuthorizationHeader(client, Request.Cookies["Token"]);

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
            todo_library.Helpers.StaticHelpers.SetAuthorizationHeader(client, Request.Cookies["Token"]);

            HttpResponseMessage response = await client.PostAsJsonAsync($"todos/addToDo?title={toDo.Title}", toDo.Title);

            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View("Index");
        }

        public async Task<IActionResult> DeleteToDo(int id)
        {
            todo_library.Helpers.StaticHelpers.SetAuthorizationHeader(client, Request.Cookies["Token"]);

            HttpResponseMessage response = await client.DeleteAsync($"todos/deleteToDo/{id}");

            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View("Index");
        }

        public async Task<IActionResult> ChangeIsDone(int id)
        {

            todo_library.Helpers.StaticHelpers.SetAuthorizationHeader(client, Request.Cookies["Token"]);

            HttpResponseMessage response = await client.GetAsync($"todos/changeIsDone/{id}");

            if (response.IsSuccessStatusCode) return RedirectToAction("Index");

            return View("Index");
        }
    }
}

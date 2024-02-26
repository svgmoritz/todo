using Microsoft.AspNetCore.Mvc;
using todo_library.Models;

namespace todo_app.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient client;

        public AuthController()
        {
            client = todo_library.Helpers.StaticHelpers.Create();
        }

        public async Task<IActionResult> Register([Bind("Email", "Password") ] UserDto user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("auth/register", user);

            if (response.IsSuccessStatusCode) return View("Login");

            return View("Register");
        }

        public async Task<IActionResult> Login([Bind("Email", "Password")] UserDto user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("auth/login", user);

            if (response.IsSuccessStatusCode) return View();

            return View("Login");
        }
    }
}


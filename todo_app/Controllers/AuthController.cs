using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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

        public async Task<IActionResult> Register([Bind("Email", "Password")] UserDto user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("auth/register", user);

            if (response.IsSuccessStatusCode) return View("Login");

            return View("Register");
        }

        public async Task<IActionResult> Login([Bind("Email", "Password")] UserDto user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("auth/login", user);
                                              
            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(token)) return RedirectToAction("Login", "Auth");

                JObject jObj = JObject.Parse(token);
                token = jObj["accessToken"].ToString();

                Response.Cookies.Append("Token", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });

                return RedirectToAction("Index", "ToDo");
            }

            return View("Login");
        }
    }
}


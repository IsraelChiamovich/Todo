using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using exm.Models;

namespace exm.Controllers
{
    public class TodoController(IHttpClientFactory clientFactory) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://dummyjson.com/todos");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var todos = JsonSerializer.Deserialize<Todos>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(todos.todos);
            }

            return View("Error");
        }

        public async Task<IActionResult> Details(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"https://dummyjson.com/todos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var todo = JsonSerializer.Deserialize<Todo>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(todo);
            }

            return View("Error");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Todo todo)
        {
            var httpClient = clientFactory.CreateClient();
            var json = JsonSerializer.Serialize(todo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://dummyjson.com/todos/add", content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Success");
            }

            return View("Error");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.GetAsync($"https://dummyjson.com/todos/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var todo = JsonSerializer.Deserialize<Todo>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return View(todo);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Todo todo)
        {
            var httpClient = clientFactory.CreateClient();
            var json = JsonSerializer.Serialize(todo);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PutAsync($"https://dummyjson.com/todos/{todo.id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Success");
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var httpClient = clientFactory.CreateClient();
            var response = await httpClient.DeleteAsync($"https://dummyjson.com/todos/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Success");
            }

            return View("Error");
        }

        public IActionResult Success()
        {
            ViewBag.Message = "Success!";
            return View();
        }

        public IActionResult Error()
        {
            ViewBag.Message = "error";
            return View();
        }
    }
}

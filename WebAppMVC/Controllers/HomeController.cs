using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Uri baseAddress = new Uri("http://localhost:5246/api");
        HttpClient client;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> LoginUser(UserLogin user)
        {
            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user),Encoding.UTF8,"application/json");
            using (var response = await client.PostAsync(client.BaseAddress+"/Students/login",stringContent))
            {
                string token = await response.Content.ReadAsStringAsync();
                if (token== "Student details not found") {
                    ViewBag.Message = "Incorrect User Id or Password";
                    return Redirect("~/Home/Index");
                }
                HttpContext.Session.SetString("jWToken", token);
            }
            return Redirect("~/DashBoard/Index");
        }

        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return Redirect("~/Home/Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
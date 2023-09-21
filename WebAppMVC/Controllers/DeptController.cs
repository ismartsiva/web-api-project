using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class DeptController : Controller
    {
        
        Uri baseAdd = new Uri("http://localhost:5246/api/Departments/");
        HttpClient client;
        public DeptController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAdd;
        }
        public async Task<IActionResult> Index()
        {
            var depts = await GetDepartments();
            return View(depts);
        }

        public async Task<List<DeptViewModel>> GetDepartments()
        {
            List<DeptViewModel> modelList = new List<DeptViewModel>();
            var accessToken = HttpContext.Session.GetString("jWToken");
            var url = client.BaseAddress;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);
            modelList = JsonConvert.DeserializeObject<List<DeptViewModel>>(jsonStr);
            return modelList;
        }

        
    }
}

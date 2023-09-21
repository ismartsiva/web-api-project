using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Policy;
using System.Text;
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

        public async Task<List<Department>> GetDepartments()
        {
            List<Department> modelList = new List<Department>();
            //var accessToken = HttpContext.Session.GetString("jWToken");
            //var url = client.BaseAddress;
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(client.BaseAddress);
            modelList = JsonConvert.DeserializeObject<List<Department>>(jsonStr);
            return modelList;
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department dept)
        {
            string data = JsonConvert.SerializeObject(dept);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Edit(long id)
        {
            Department modelList = new Department();
            string jsonStr = await client.GetStringAsync(client.BaseAddress +id.ToString());
            modelList = JsonConvert.DeserializeObject<Department>(jsonStr);
            return View(modelList);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department dept)
        {
            string data = JsonConvert.SerializeObject(dept);
            var id = dept.Id;
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(client.BaseAddress+ id.ToString(), content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Details(long Id)
        {
            Department modelList = new Department();
            string jsonStr = await client.GetStringAsync(client.BaseAddress + Id.ToString());
            modelList = JsonConvert.DeserializeObject<Department>(jsonStr);
            return View(modelList);
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using WebAppMVC.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAppMVC.Controllers
{
    public class StudentController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5246/api/Students/");
        Uri baseAdd = new Uri("http://localhost:5246/api/");
        HttpClient client;
        public StudentController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }

        public async Task<IActionResult> Index()
        {
            var students = await GetStudents();
            return View(students);  
        }
        public async Task<List<StuddentsViewModel>> GetStudents()
        {
            List<StuddentsViewModel> modelList = new List<StuddentsViewModel>();
            //HttpResponseMessage response = client.GetAsync(client.BaseAddress+ "/Students").Result;
            //if(response.IsSuccessStatusCode)
            //{
            //    string data = response.Content.ReadAsStringAsync().Result;
            //    modelList = JsonConvert.DeserializeObject<List<StuddentsViewModel>>(data);

            //}
            //return View(modelList);


            var accessToken = HttpContext.Session.GetString("jWToken");
            var url = client.BaseAddress;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);
            modelList = JsonConvert.DeserializeObject<List<StuddentsViewModel>>(jsonStr);
            return modelList;
        }


        public async Task<ActionResult> Edit(int Id)
        {


            StuddentsViewModel modelList = new StuddentsViewModel();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress +Id.ToString()).Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<StuddentsViewModel>(data);

            }
            return View("Create",modelList);
        }

        [HttpPost]
        public ActionResult Edit(StuddentsViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(client.BaseAddress+ "/Students/"+model.Id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Create",model);
        }
        public ActionResult Create()
        {
            List<DeptViewModel> modelList = new List<DeptViewModel>();
            HttpResponseMessage response = client.GetAsync(baseAdd+ "Departments").Result;
            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<DeptViewModel>>(data);

                ViewBag["deptNames"] = modelList;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(StuddentsViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);   
            StringContent content = new StringContent(data,Encoding.UTF8,"application/json");
            HttpResponseMessage response = client.PostAsync(client.BaseAddress+ "/Students", content).Result;
            if(response.IsSuccessStatusCode) {
                return RedirectToAction("Index");
            }
            return View();
        }

        
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using WebApplication1.login;
using WebAppMVC.Models;

namespace WebAppMVC.Controllers
{
    public class StudentController : Controller
    {
        Uri baseAddress = new Uri("http://localhost:5246/api");
        HttpClient client;
        public StudentController()
        {
            client = new HttpClient();
            client.BaseAddress = baseAddress;
        }
        public IActionResult Index()
        {
            List<StuddentsViewModel> modelList = new List<StuddentsViewModel>();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress+ "/Students").Result;
            if(response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                modelList = JsonConvert.DeserializeObject<List<StuddentsViewModel>>(data);

            }
            return View(modelList);
        }


        public ActionResult Edit(int Id)
        {
            StuddentsViewModel modelList = new StuddentsViewModel();
            HttpResponseMessage response = client.GetAsync(client.BaseAddress + "/Students/"+Id).Result;
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

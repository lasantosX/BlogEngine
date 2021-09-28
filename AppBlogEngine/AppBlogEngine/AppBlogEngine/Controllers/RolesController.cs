using AppBlogEngine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppBlogEngine.Controllers
{
    public class RolesController : Controller
    {
        //URL API 
        string BaseUrl = "https://localhost:44387/";

        // GET: api/Roles
        public async Task<ActionResult> Index()
        {

            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            List<Roles> rolesList = new List<Roles>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Roles");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    rolesList = JsonConvert.DeserializeObject<List<Roles>>(appResponse);
                }

                //Muestra la lista de todos los Appointments               
                return View(rolesList);
            }
        }

        //Crear nuevo User
        //public async Task<ActionResult> Create()
        public ActionResult Create()
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            return View();
        }

        // POST: api/Roles
        [HttpPost]
        public ActionResult Create(Roles Roles)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl + "api/Roles");
                var postTask = client.PostAsJsonAsync<Roles>("Roles", Roles);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Error, contact the administrator.");

            return View(Roles);
        }

        //Modificar usuario        
        public ActionResult Edit(int id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Roles Roles = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/Roles/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Roles>();
                    readTask.Wait();
                    Roles = readTask.Result;
                }
            }

            return View(Roles);
        }

        // PUT: api/Roles/5
        [HttpPost]
        public ActionResult Edit(Roles Roles)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //HTTP POST
                var putTask = client.PutAsJsonAsync($"api/Roles/{Roles.idRole}", Roles);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(Roles);
        }

        //Details Roles
        public ActionResult Details(int Id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Roles Roles = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/Roles/" + Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Roles>();
                    readTask.Wait();
                    Roles = readTask.Result;
                }
            }

            return View(Roles);
        }

        //Delete 
        public ActionResult Delete(int Id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Roles Roles = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Get
                var responseTask = client.GetAsync("api/Roles/" + Id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Roles>();
                    readTask.Wait();
                    Roles = readTask.Result;
                }
            }

            return View(Roles);
        }

        // DELETE: api/Roles/5
        [HttpPost]
        public ActionResult Delete(Roles Roles, int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Delete
                var deleteTask = client.DeleteAsync($"api/Roles/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(Roles);
        }
    }
}

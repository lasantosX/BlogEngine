using AppBlogEngine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppBlogEngine.Controllers
{
    public class MyPublishedController : Controller
    {
        //URL API 
        string BaseUrl = "https://localhost:44387/";

        // GET: api/Published
        public async Task<ActionResult> Index()
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            string vv = HttpContext.Session.GetString("idUser");
            long id = long.Parse(HttpContext.Session.GetString("idUser")); // aqui obtener el id del usuario

            List<Published> PublishedList = new List<Published>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/published");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    PublishedList = JsonConvert.DeserializeObject<List<Published>>(appResponse);
                }




                //Hacer foreach y eliminar registros que no sean igual a id
                List<Published> PublishedListTemp = new List<Published>();
                foreach (var publishL in PublishedList)
                {
                    if (publishL.idUser == id)
                    {
                        PublishedListTemp.Add(publishL);
                    }
                }

                //Muestra la lista de todos los Appointments               
                return View(PublishedListTemp);
            }


        }


        //Create new publisher
        //public async Task<ActionResult> Create()
        public ActionResult Create()
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");



            return View();
        }

        // POST: api/Published
        [HttpPost]
        public ActionResult Create(Published published)
        {
            

            published.idUser = int.Parse(HttpContext.Session.GetString("idUser"));
            published.status = 0; 

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl + "api/published");
                var postTask = client.PostAsJsonAsync<Published>("published", published);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Error, contact the administrator.");

            return View(published);
        }

        //Edit Published        
        public ActionResult Edit(int id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Published published = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/published/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Published>();
                    readTask.Wait();
                    published = readTask.Result;
                }
            }

            StatePublished states = new StatePublished();
            List<StatePublished> statePublishedL = states.ListStates();

            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = statePublishedL.Select(c => new SelectListItem { Value = c.value.ToString(), Text = c.nameState });

            ViewBag.listStatus = item;
            //--------------------------------------------------------------------------------------------------------------------------------

            return View(published);
        }

        // PUT: api/Published/5
        [HttpPost]
        public ActionResult Edit(Published published)
        {
            published.idUser = int.Parse(HttpContext.Session.GetString("idUser"));

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //HTTP POST
                var putTask = client.PutAsJsonAsync($"api/Published/{published.idPublished}", published);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(published);
        }

        //Details Users
        public ActionResult Details(int Id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Published published = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/published/" + Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Published>();
                    readTask.Wait();
                    published = readTask.Result;
                }
            }

            return View(published);
        }

        //Delete
        public ActionResult Delete(int Id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Published published = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Get
                var responseTask = client.GetAsync("api/published/" + Id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Published>();
                    readTask.Wait();
                    published = readTask.Result;
                }
            }

            return View(published);
        }

        // DELETE: api/Roles/5
        [HttpPost]
        public ActionResult Delete(Published published, int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Delete
                var deleteTask = client.DeleteAsync($"api/Published/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(published);
        }

        //Edit Published        
        public ActionResult SubmitPosts(int id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Published published = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/published/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Published>();
                    readTask.Wait();
                    published = readTask.Result;
                }
            }

            //StatePublished states = new StatePublished();
            //List<StatePublished> statePublishedL = states.ListStates();
            List<StatePublished> statePublishedL = new List<StatePublished>()
            {
                new StatePublished() { value = 0, nameState = "Created" },
                new StatePublished() { value = 1, nameState = "Send" }
            };

            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = statePublishedL.Select(c => new SelectListItem { Value = c.value.ToString(), Text = c.nameState });

            ViewBag.listStatus = item;
            //--------------------------------------------------------------------------------------------------------------------------------


            return View(published);
        }

        // PUT: api/Published/5
        [HttpPost]
        public ActionResult SubmitPosts(Published published)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //HTTP POST
                var putTask = client.PutAsJsonAsync($"api/Published/{published.idPublished}", published);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(published);
        }

    }
}

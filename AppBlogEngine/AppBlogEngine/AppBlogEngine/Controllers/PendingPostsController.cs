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
    public class PendingPostsController : Controller
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
           
                return View(PublishedList);
            }


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

            List<StatePublished> statePublishedL = new List<StatePublished>()
            {
                new StatePublished() { value = 2, nameState = "Approved" },
                new StatePublished() { value = 3, nameState = "Reyected" }
            };

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
                var responseTask = client.GetAsync("api/Published/" + Id.ToString());
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

    }
}

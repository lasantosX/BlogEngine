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
    public class CommentsController : Controller
    {
        //URL API 
        string BaseUrl = "https://localhost:44387/";


        //Modificar Lista de comentarios de publicacion        
        public async Task<ActionResult> Index(int id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");



            if (id == 0)
            {
                id = int.Parse(HttpContext.Session.GetString("idPublushedC"));
            }
            else
            {
                HttpContext.Session.SetString("idPublushedC", id.ToString());
            }

            List<Comments> CommentsList = new List<Comments>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/comments");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    CommentsList = JsonConvert.DeserializeObject<List<Comments>>(appResponse);
                }

                //Hacer foreach y eliminar registros que no sean igual a id
                List<Comments> CommentsListTemp = new List<Comments>();
                foreach (var commentL in CommentsList)
                {
                    if (commentL.idComments != id)
                    {
                        CommentsListTemp.Add(commentL);
                    }
                }

                //Muestra la lista de todos los Appointments               
                return View(CommentsListTemp);
            }
        }

        public async Task<ActionResult> Create()
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            List<Comments> roleList = new List<Comments>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Comments");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    roleList = JsonConvert.DeserializeObject<List<Comments>>(appResponse);
                }

            }

            

            return View();
        }

        // POST: api/Comments
        [HttpPost]
        public ActionResult Create(Comments Comments)
        {
            Comments.idPublished = int.Parse(HttpContext.Session.GetString("idPublushedC"));

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl + "api/Comments");
                var postTask = client.PostAsJsonAsync<Comments>("Comments", Comments);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Error, contact the administrator.");

            return View(Comments);
        }




    }
}

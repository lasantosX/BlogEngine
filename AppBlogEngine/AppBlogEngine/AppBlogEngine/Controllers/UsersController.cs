using AppBlogEngine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AppBlogEngine.Controllers
{
    public class UsersController : Controller
    {

        //URL API 
        string BaseUrl = "https://localhost:44387/";

        // GET: api/Users
        public async Task<ActionResult> Index()
        {

            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //string ss = TempData["manageMenu"].ToString();

            List<Users> usersList = new List<Users>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Users");
                if (Res.IsSuccessStatusCode)
                {
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    usersList = JsonConvert.DeserializeObject<List<Users>>(appResponse);
                }

                //Muestra la lista de todos los Appointments               
                return View(usersList);
            }
        }

        //Crear nuevo User
        //public async Task<ActionResult> Create()
        public async Task<ActionResult> Create()
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            List<Roles> roleList = new List<Roles>();
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
                    roleList = JsonConvert.DeserializeObject<List<Roles>>(appResponse);
                }

            }

            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = roleList.Select(c => new SelectListItem { Value = c.idRole.ToString(), Text = c.roleName });

            ViewBag.listRole = item;
            //--------------------------------------------------------------------------------------------------------------------------------

            return View();
        }

        public async Task<List<Users>> ListUser()
        {
            List<Users> usersList = new List<Users>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                //Llena todos los Appointments uzando el HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Users");
                if (Res.IsSuccessStatusCode)
                {
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    usersList = JsonConvert.DeserializeObject<List<Users>>(appResponse);
                }

                return usersList;
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult> Create(Users users)
        {
            List<Users> usersList = await ListUser();


            foreach (var item in usersList)
            {
                if (item.userName == users.userName)
                {
                    //ModelState.AddModelError(string.Empty, "The user who tries to enter is already registered.");

                    //break;
                    return RedirectToAction("Create");
                }
            }

            users.password = Encryted(users.password);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl + "api/Users");
                var postTask = client.PostAsJsonAsync<Users>("users", users);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Error, contact the administrator.");

            return View(users);
        }

        //Modificar usuario        
        public async Task<ActionResult> Edit(int id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Users users = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/users/" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Users>();
                    readTask.Wait();
                    users = readTask.Result;
                }
            }

            List<Roles> roleList = new List<Roles>();
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
                    roleList = JsonConvert.DeserializeObject<List<Roles>>(appResponse);
                }

            }

            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = roleList.Select(c => new SelectListItem { Value = c.idRole.ToString(), Text = c.roleName });

            ViewBag.listRole = item;
            //--------------------------------------------------------------------------------------------------------------------------------


            return View(users);
        }

        // PUT: api/Users/5
        [HttpPost]
        public ActionResult Edit(Users users)
        {
            users.password = Encryted(users.password);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //HTTP POST
                var putTask = client.PutAsJsonAsync($"api/Users/{users.idUser}", users);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(users);
        }

        //Details Users
        public ActionResult Details(int Id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Users users = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                //Http Get
                var responseTask = client.GetAsync("api/Users/" + Id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Users>();
                    readTask.Wait();
                    users = readTask.Result;
                }
            }

            return View(users);
        }

        //Delete 
        public ActionResult Delete(int Id)
        {
            TempData["manageMenu"] = HttpContext.Session.GetString("manageMenu");
            TempData["firstLastName"] = HttpContext.Session.GetString("firstLastName");
            TempData["rolName"] = HttpContext.Session.GetString("rolName");
            TempData["idUser"] = HttpContext.Session.GetString("idUser");

            //Models
            Users users = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Get
                var responseTask = client.GetAsync("api/users/" + Id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Users>();
                    readTask.Wait();
                    users = readTask.Result;
                }
            }

            return View(users);
        }

        // DELETE: api/Users/5
        [HttpPost]
        public ActionResult Delete(Users users, int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                //Http Delete
                var deleteTask = client.DeleteAsync($"api/users/" + id.ToString());
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(users);
        }

        public static string Encryted(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }


        public static string Decryted(string password)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(password);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }

    }
}

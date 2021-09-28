using AppBlogEngine.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AppBlogEngine.Controllers
{
   
    public class ManageUserController : Controller
    {
        //const string SessionMenu = "manageMenu";
        //const string SessionfirstLastName = "firstLastName";
        //const string SessionrolName = "rolName";

        //URL API 
        string BaseUrl = "https://localhost:44387/";

        //Crear nuevo User
        //public async Task<ActionResult> Create()
        public async Task<ActionResult> Create()
        {
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

            List<Roles> roleListTem = new List<Roles>();
            foreach (var itemX in roleList)
            {
                if (itemX.roleName != "Administrator")
                {
                    roleListTem.Add(itemX);
                }
            }


            //--------------------------------------------------------------------------------------------------------------------------------
            IEnumerable<SelectListItem> item = roleListTem.Select(c => new SelectListItem { Value = c.idRole.ToString(), Text = c.roleName });

            ViewBag.listRole = item;
            //--------------------------------------------------------------------------------------------------------------------------------

            return View();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult> Create(Users users)
        {
            List<Users> usersList = await ListUser();

            int count = 0;
            foreach (var item in usersList)
            {
                if (item.userName == users.userName)
                {
                    ModelState.AddModelError(string.Empty, "The user who tries to enter is already registered.");
                    count++;
                    break;
                    
                }
            }

            if (count == 0)
            {
                ManageUsers(users);

                users.password = Encryted(users.password);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(BaseUrl + "api/Users");
                    var postTask = client.PostAsJsonAsync<Users>("users", users);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        //return RedirectToAction("Index");
                        return RedirectToAction("Index", "Home");
                    }
                }

                // ModelState.AddModelError(string.Empty, "Error, contact the administrator.");
                return View(users);

            }
            else
            {
                return RedirectToAction("Create", "ManageUser");
            }

           
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

        public ActionResult Login()
        {          
            return View();
        }

        // GET: api/Users
        [HttpPost]
        public async Task<ActionResult> Login(string userName, string password)
        {

            HttpContext.Session.Clear();

            Users users = null;
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
                    var token = Res.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    //Si Res = true entra y asigna los datos
                    var appResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializar el API y almacena los datos
                    usersList = JsonConvert.DeserializeObject<List<Users>>(appResponse);


                    if (usersList.Count > 0)
                    {
                        foreach (var item in usersList)
                        {
                            if (item.userName == userName && Decryted(item.password) == password)
                            {
                                users = item;

                                ManageUsers(users);

                                //return View(usersList);
                                return RedirectToAction("Index", "Home");

                                //break;
                            }
                            else
                            {
                                if (item.userName == userName && Decryted(item.password) != password)
                                {
                                    ModelState.AddModelError(string.Empty, "Incorrect password!.");
                                    //return RedirectToAction("Login", "ManageUser");
                                }
                            }
                        }

                    }

                    HttpContext.Session.SetString("ExpiryToken", token);
                }


                return RedirectToAction("Create");
                       
            }
        }

        public Roles SearchRole(long Id)
        {
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

            return Roles;
        }

        public void ManageUsers(Users users)
        {
            Roles roles = null;

            if (users != null)
            {
                HttpContext.Session.SetString("manageMenu", users.userName);
                HttpContext.Session.SetString("firstLastName", users.firstLastName);
                HttpContext.Session.SetString("idUser", users.idUser.ToString());
                TempData["manageMenu"] = users.userName;
                TempData["firstLastName"] = users.firstLastName;
                roles = SearchRole(users.idRole);
            }
            //else
            //{
            //    HttpContext.Session.SetString("manageMenu", null);
            //    HttpContext.Session.SetString("firstLastName", null);
            //    HttpContext.Session.SetString("idUser", "-1");
            //    TempData["manageMenu"] = null;
            //    TempData["firstLastName"] = null;
            //}

            if (roles != null)
            {
                HttpContext.Session.SetString("rolName", roles.roleName);
                TempData["rolName"] = roles.roleName;
            }
            //else
            //{
            //    HttpContext.Session.SetString("rolName", null);
            //    TempData["rolName"] = null;
            //}         
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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

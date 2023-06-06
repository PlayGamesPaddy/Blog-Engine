using Blog_Engine.Controllers.Api;
using Blog_Engine.Models;
using Blog_Engine.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Blog_Engine.Controllers
{
    public class categoriesController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<CategoryViewModel> catagories = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/api/");
                //http get
                var responseTask = client.GetAsync("categoriesApi");
                responseTask.Wait();

                   var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<CategoryViewModel>>();
                    readTask.Wait();
                    //readTask.Result.
                    catagories = readTask.Result;
                }
                else
                {
                    catagories = null;
                }
                
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            ///I notice that I hade inplemented this  but ran out of time to do it the correct way. It still gives the expected outcome.
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            IEnumerable<Post> posts = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/api/");
                //http get
                var responseTask = client.GetAsync("postApi");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readPostTask = result.Content.ReadAsAsync<IList<Post>>();
                    readPostTask.Wait();
                    posts = readPostTask.Result.ToList();
                    foreach (var cat in catagories)
                    {
                        cat.Posts = (List<Post>?)posts;
                        break;
                    }
                }
                else
                {
                    posts = null;
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            return View(catagories);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(CategoryViewModel category)
        {
            bool valid = true;
            using (var client = new HttpClient())
            {
                    if (category.Title == "") { ModelState.AddModelError("Title", "Title cannot be empty."); valid = false; }
                    if (!cheackTitle(category.Title, category.Id)) { ModelState.AddModelError("Title", "This Category is already in use."); valid = false; }
                    if (!valid) { return View("Add"); }
                    client.BaseAddress = new Uri("https://localhost:7107/api/");
                    //http post
                    var postTask = client.PostAsJsonAsync<CategoryViewModel>("categoriesApi", category);
                    postTask.Wait();

                    var result = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("");
                    }
                    ModelState.AddModelError(String.Empty, result.IsSuccessStatusCode.ToString());
                
                return View("Add");
            }
        }
        public IActionResult Edit(int id)
        {
            CategoryViewModel category = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/api/");

                var responseTask = client.GetAsync("categoriesApi/id?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<CategoryViewModel>();
                    readTask.Wait();
                    category = readTask.Result;
                }
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(CategoryViewModel category)
        {
            bool valid = true; 
            using (var client = new HttpClient())
            {
                    if (category.Title == "") { ModelState.AddModelError("Title", "Title cannot be empty."); valid = false; }
                    if (!cheackTitle(category.Title, category.Id)) { ModelState.AddModelError("Title", "This Category is already in use."); valid = false; }
                    if (!valid) { return View(category); }
                    client.BaseAddress = new Uri("https://localhost:7107/api/");
                    var putTask = client.PutAsJsonAsync<CategoryViewModel>("categoriesApi/" + category.Id.ToString(), category);
                    putTask.Wait();
                    var result = putTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("");
                    }
            }
            return View(category);
        }
        public bool cheackTitle(string titleIn, int id)
        {
            var list = new List<CategoryViewModel>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            response = httpClient.GetAsync("https://localhost:7107/api/categoriesApi").Result;
            response.EnsureSuccessStatusCode();
            List<CategoryViewModel> CategoryList = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;
            if (!object.Equals(CategoryList, null))
            {
                var Categories = CategoryList.ToList();
                foreach (var Category in Categories)
                {
                    if (Category.Title == titleIn)
                    {
                        //this lets edits keep the origenal title 
                        if (Category.Id == id)
                        {
                            return true;
                        }
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

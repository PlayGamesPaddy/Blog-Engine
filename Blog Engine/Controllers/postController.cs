using Microsoft.AspNetCore.Mvc;
using Blog_Engine.Models.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Web.Mvc;

namespace Blog_Engine.Controllers
{
    public class postController : Controller
    {
        public IActionResult Index()
        {
            IEnumerable<PostViewModel> post = null;
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/api/");
                var responceTask = client.GetAsync("postApi");
                responceTask.Wait();
                var result = responceTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<PostViewModel>>();
                    readTask.Wait();
                    post = readTask.Result;
                }
                else
                {
                    post = null;
                }
            }
            return View(post);
        }
        public ActionResult Add()
        {
            ViewBag.Categories = getCategories(0);
            return View();
        }
        [HttpPost]
        public IActionResult Add(PostViewModel post)
        {
            bool valid = true;
            using (var client = new HttpClient())
            {
                if (post.Title == ""| post.Title == null) { ModelState.AddModelError("Title", "Title cannot be empty."); valid = false; }
                if (!cheackTitle(post.Title, 0)) { ModelState.AddModelError("Title", "A post already has this title."); valid = false; }
                if (post.Content == ""| post.Content == null) { ModelState.AddModelError("Content", "Content cannot be empty."); valid = false; }
                if (post.publicationDate == new DateTime()) { ModelState.AddModelError("publicationDate", "Title cannot be empty."); valid = false; }
                if (!valid) { ViewBag.Categories = getCategories(post.Id); return View(post); }
                client.BaseAddress = new Uri("https://localhost:7107/api/");
                var postTask = client.PostAsJsonAsync<PostViewModel>("postApi", post);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Categories");
                }
                ModelState.AddModelError(String.Empty, result.IsSuccessStatusCode.ToString());
                ViewBag.Categories = getCategories(post.Id);
                return View(post);
            }
        }
        public IActionResult Edit(int id)
        {
            PostViewModel post = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7107/api/");
                var responseTask = client.GetAsync("postApi/id?id="+id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<PostViewModel>();
                    readTask.Wait();
                    post = readTask.Result;
                }
            }
            ViewBag.Categories = getCategories(post.CategoryId);
            return View(post);
        }
        [HttpPost]
        public IActionResult Edit(PostViewModel post)
        {
            bool valid = true;
            using (var client = new HttpClient())
            {
                if (post.Title == "" | post.Title == null) { ModelState.AddModelError("Title", "Title cannot be empty."); valid = false; }
                if (!cheackTitle(post.Title,post.Id)) { ModelState.AddModelError("Title", "A post already has this title."); valid = false; }
                if (post.Content == "" | post.Content == null) { ModelState.AddModelError("Content", "Content cannot be empty."); valid = false; }
                if (post.publicationDate == new DateTime()) { ModelState.AddModelError("publicationDate", "Title cannot be empty."); valid = false; }
                if (!valid) { ViewBag.Categories = getCategories(post.Id); return View(post); }
                client.BaseAddress = new Uri("https://localhost:7107/api/");
                var putTask = client.PutAsJsonAsync<PostViewModel>("postApi/"+post.Id.ToString(), post);
                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Categories");
                }
                //return RedirectToAction("Index");
                ViewBag.Categories = getCategories(post.Id);
                return View(post);
            }
        }
        //This method is used to get List<SelectListItem> for the categories dropdown list
        public List<SelectListItem> getCategories(int id)
        {
            var list = new List<CategoryViewModel>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            response = httpClient.GetAsync("https://localhost:7107/api/categoriesApi").Result;
            response.EnsureSuccessStatusCode();
            List<CategoryViewModel> categoriesList = response.Content.ReadAsAsync<List<CategoryViewModel>>().Result;
            if (!object.Equals(categoriesList, null))
            {
                var categories = categoriesList.ToList();
                List <SelectListItem> categoriesSelect = new List<SelectListItem>();
                foreach (var category in categoriesList)
                {
                    categoriesSelect.Add(new SelectListItem
                    {
                        Text = category.Title,
                        Value = category.Id.ToString(),
                        Selected = (category.Id==id)
                    });
                }
                return categoriesSelect;
            }
            else
            {
                return null;
            }
        }
        public bool cheackTitle(string titleIn, int id)
        {
            var list = new List<PostViewModel>();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            response = httpClient.GetAsync("https://localhost:7107/api/postApi").Result;
            response.EnsureSuccessStatusCode();
            List<PostViewModel> postList = response.Content.ReadAsAsync<List<PostViewModel>>().Result;
            if (!object.Equals(postList, null))
            {
                var posts = postList.ToList();
                foreach(var post in posts)
                {
                    if(post.Title == titleIn)
                    {
                        //this lets edits keep the origenal title 
                        if (post.Id == id)
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

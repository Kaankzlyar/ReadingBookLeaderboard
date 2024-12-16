using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using LeaderboardFrontend.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace LeaderboardFrontend.Controllers
{
    public class UsersController : Controller
    {
        private readonly string baseURL = "https://localhost:7172";
        

        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }
       
       

        public async Task<IActionResult> Index()
        {
            List<LeaderboardViewModel> listUsers = new List<LeaderboardViewModel>();

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseURL + "/api/BookItemsApi");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // API'den verileri çekme
                HttpResponseMessage getData = await _httpClient.GetAsync("");

                if (getData.IsSuccessStatusCode)
                {
                    // Asenkron olarak içeriği oku ve deserialize et
                    string result = await getData.Content.ReadAsStringAsync();
                    listUsers = JsonConvert.DeserializeObject<List<LeaderboardViewModel>>(result);
                }
                else
                {
                    return RedirectToAction("ErrorPage"); // Hata sayfasına yönlendir
                }
            }

            // Verileri View'e aktar
            return View(listUsers);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> PostLeaderboardItem(LeaderboardViewModel log)
        {
            if (!ModelState.IsValid)
            {
                return View(log);
            }

            var LeaderboardItem = new LeaderboardViewModel
            {
                Id = log.Id,
                Username = log.Username,
                TotalPages = log.TotalPages,
                TodayPages = log.TodayPages,
            };

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseURL + "/api/BookItemsApi");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage postData = await _httpClient.PostAsJsonAsync("", LeaderboardItem);

                if (postData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("ErrorPage");
                }
            }
        }


       

        public async Task<IActionResult> Delete(long id)
        {
            
            if (id == 0)
            {
                return RedirectToAction("ErrorPage");
            }

            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseURL + "/api/BookItemsApi/");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                
                HttpResponseMessage getResponse = await _httpClient.GetAsync($"GetBookItems{id}");

                if (!getResponse.IsSuccessStatusCode)
                {
                    var errorContent = await getResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Hata: {getResponse.StatusCode}, İçerik: {errorContent}");
                    return RedirectToAction("ErrorPage");
                }

                var result = await getResponse.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<LeaderboardViewModel>(result);

                
                return View(model);
            }
        }


        public async Task<IActionResult> DeleteConfirmed(long id)
        {

            if (id == 0)
            {
                return RedirectToAction("ErrorPage"); 
            }
            using (var _httpClient = new HttpClient())
            {
                _httpClient.BaseAddress = new Uri(baseURL + "/api/BookItemsApi/");
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                
                HttpResponseMessage deleteResponse = await _httpClient.DeleteAsync($"DeleteBookItems{id}");

                if (!deleteResponse.IsSuccessStatusCode)
                {
                    var errorContent = await deleteResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Hata: {deleteResponse.StatusCode}, İçerik: {errorContent}");
                    return RedirectToAction("ErrorPage");
                }

            }

            return RedirectToAction("Index"); // Silme işlemi başarılıysa listeye dön
        }







        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}

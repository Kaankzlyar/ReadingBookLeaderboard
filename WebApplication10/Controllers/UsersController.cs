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
        

       

        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}

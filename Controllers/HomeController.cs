using System.Diagnostics;
using DivinePromo.Models;
using DivinePromo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DivinePromo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Головний маршрут ("/") тепер повертає Merch.cshtml
        public IActionResult Index()
        {
            return View("Merch");
        }

        // Дія для сторінки Merch
        public IActionResult Merch()
        {
            return View();
        }

        // Дія для сторінки Tour
        public IActionResult Tour()
        {
            return View();
        }

        // Дія для сторінки Home (Галерея)
        public IActionResult Home()
        {
            return View();
        }

        // Дія для сторінки підписки
        public IActionResult Subscribe()
        {
            return View();
        }

        // Дія для обробки підписки
        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email) || !model.Email.Contains("@"))
                {
                    return Json(new { success = false, message = "Please enter a valid email address." });
                }

                // Перевіряємо чи email вже існує
                var existingSubscriber = await _context.Subscribers
                    .FirstOrDefaultAsync(s => s.Email == model.Email);

                if (existingSubscriber != null)
                {
                    return Json(new { success = false, message = "This email is already subscribed." });
                }

                // Додаємо нового підписника
                var subscriber = new Subscriber
                {
                    Email = model.Email,
                    SubscriptionDate = DateTime.Now
                };

                _context.Subscribers.Add(subscriber);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"New subscriber added: {model.Email}");

                return Json(new { success = true, message = "Thank you for subscribing! You will be notified about upcoming events." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error subscribing email: {Email}", model.Email);
                return Json(new { success = false, message = "An error occurred. Please try again later." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    // Модель для отримання даних з форми
    public class SubscribeModel
    {
        public string Email { get; set; }
    }
}
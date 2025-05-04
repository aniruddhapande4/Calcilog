using Microsoft.AspNetCore.Mvc;
using CalciLog.Models;
using CalciLog.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CalciLog.Controllers
{
    public class ConsumerController : Controller
    {
        private readonly AppDbContext _context;

        public ConsumerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return View();

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // ✅ Check if consumer already exists for this user
            var existingConsumer = _context.Consumers
                .FirstOrDefault(c => c.Name == name && c.CreatedByUserId == userId);

            if (existingConsumer != null)
            {
                TempData["Message"] = "Consumer already exists!";
                TempData["MessageType"] = "warning";

                // ✅ Redirect to calculator for existing consumer
                return RedirectToAction("Calculator", new { id = existingConsumer.Id });
            }

            var consumer = new Consumer
            {
                Name = name,
                CreatedByUserId = userId.Value
            };

            _context.Consumers.Add(consumer);
            _context.SaveChanges();

            TempData["Message"] = "Consumer created successfully!";
            TempData["MessageType"] = "success";

            return RedirectToAction("Calculator", new { id = consumer.Id });
        }

        public IActionResult Calculator(int id)
        {
            var consumer = _context.Consumers.Find(id);
            if (consumer == null) return NotFound();

            ViewBag.Consumer = consumer;
            return View();
        }

        [HttpPost]
        public IActionResult Calculator(int consumerId, string expression)
        {
            var result = new System.Data.DataTable().Compute(expression, null);
            var record = new CalculationRecord
            {
                ConsumerId = consumerId,
                Expression = expression,
                Result = result.ToString(),
                Timestamp = DateTime.Now
            };

            _context.CalculationRecords.Add(record);
            _context.SaveChanges();

            return RedirectToAction("List");
        }

        public IActionResult List(string search)
        {
            var consumers = string.IsNullOrWhiteSpace(search)
                ? _context.Consumers.Include(c => c.Records).ToList()
                : _context.Consumers
                    .Where(c => c.Name.Contains(search))
                    .Include(c => c.Records)
                    .ToList();

            ViewBag.Search = search;
            return View(consumers);
        }
    }
}

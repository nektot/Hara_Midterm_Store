using Microsoft.AspNetCore.Mvc;
using Hara_Midterm_Store.Data;
using Hara_Midterm_Store.Models;

namespace Hara_Midterm_Store.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartController(ApplicationDbContext db) { _db = db; }

        public IActionResult Index()
        {
            var items = _db.CartItems.ToList();
            ViewBag.Total = items.Sum(i => i.Price * i.Quantity);
            return View(items);
        }

        public IActionResult Add(int productId)
        {
            var product = _db.Products.Find(productId);
            if (product == null) return RedirectToAction("Index", "Products");

            var existing = _db.CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity++;
            }
            else
            {
                _db.CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(int id, int quantity)
        {
            var item = _db.CartItems.Find(id);
            if (item != null && quantity > 0)
            {
                item.Quantity = quantity;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var item = _db.CartItems.Find(id);
            if (item != null)
            {
                _db.CartItems.Remove(item);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
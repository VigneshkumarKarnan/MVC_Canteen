using Microsoft.AspNetCore.Mvc;
using EX_2_MVC.Models;
using Microsoft.EntityFrameworkCore;
using EX_2_MVC.ViewModels;


namespace EX_2_MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Step 2: Take Order → Creates a new order and redirects to Menu
        public IActionResult TakeOrder()
        {
            var order = new Order
            {
              
                Username = HttpContext.Session.GetString("Username"),

                CreatedAt = DateTime.Now,
                Status = "In Progress"
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            // Store orderId temporarily
            TempData["OrderId"] = order.Id;

            return RedirectToAction("Menu", new { orderId = order.Id });
        }

        // Step 3: Show Menu for the given OrderId
        //public IActionResult Menu(int orderId)
        //{
        //    var menuItems = _context.MenuItems.ToList(); // Make sure this table exists
        //    ViewBag.OrderId = orderId;
        //    return View(menuItems);
        //}

        public IActionResult Menu(int orderId)
        {
            // Load menu items from DB
            var menuItems = _context.MenuItems.ToList();

            ViewBag.OrderId = orderId;
            return View(menuItems);
        }

        // Step 4: Add Item to Cart for the given OrderId
        [HttpPost]
        public IActionResult AddToCart(int orderId, int menuItemId, int quantity)
        {
            var existingItem = _context.OrderItems
                .FirstOrDefault(o => o.OrderId == orderId && o.MenuItemId == menuItemId);

            if (existingItem != null)
            {
                // Update quantity if item already exists in the cart
                existingItem.Quantity += quantity;
                _context.OrderItems.Update(existingItem);
            }
            else
            {
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    MenuItemId = menuItemId,
                    Quantity = quantity
                };

                _context.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();
            return RedirectToAction("Menu", new { orderId = orderId }); // Stay on menu after adding
        }

        // Optional Step 5: View Cart (next step after adding items)
        //public IActionResult ViewCart(int orderId)
        //{
        //    var orderItems = _context.OrderItems
        //        .Include(oi => oi.MenuItem)
        //        .Where(oi => oi.OrderId == orderId)
        //        .ToList();

        //    var viewModel = orderItems.Select(oi => new CartItemViewModel
        //    {
        //        MenuItemId = oi.MenuItemId,
        //        MenuItemName = oi.MenuItem.Name,
        //        Price = oi.MenuItem.Price,
        //        Quantity = oi.Quantity
        //    }).ToList();

        //    return View(viewModel);
        //}
        public IActionResult ViewCart(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            var cartItems = order.Items.Select(i => new CartItemViewModel
            {
                MenuItemName = i.MenuItem.Name,
                Price = i.MenuItem.Price,
                Quantity = i.Quantity
            }).ToList();

            ViewBag.OrderId = orderId; //  This must be set!

            return View(cartItems);
        }


        //public IActionResult OrderSuccess(int orderId)
        //{
        //    ViewBag.OrderId = orderId;
        //    return View();
        //}
        public IActionResult OrderSummary(int orderId)
        {
            var order = _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null) return NotFound();

            return View(order);
        }


        [HttpPost]
        public IActionResult ConfirmOrder(int orderId)
        {
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == orderId);

            if (order == null || order.Items == null || !order.Items.Any())
            {
                return RedirectToAction("ViewCart", new { orderId }); // No items to confirm
            }

            order.Status = "Confirmed";
            _context.SaveChanges();

            return RedirectToAction("OrderSummary", new { orderId });
        }

        //Action to Show Payment Page
        //public IActionResult Payment(int orderId)
        //{
        //    var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

        //    if (order == null || order.Status != "Confirmed")
        //        return RedirectToAction("ViewCart", new { orderId });

        //    return View(orderId);
        //}
        public IActionResult Payment(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null || order.Status != "Confirmed")
                return RedirectToAction("ViewCart", new { orderId });

            var paymentViewModel = new PaymentViewModel
            {
                OrderId = orderId
            };

            return View(paymentViewModel); //  Pass correct model
        }


        [HttpPost]
        public IActionResult ProcessPayment(int orderId, string paymentMethod)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
                return NotFound();

            // Simulate payment processing
            order.Status = "Paid";
            _context.SaveChanges();

            ViewBag.PaymentMethod = paymentMethod;
            return View("PaymentSuccess", order); // You'll create this view next
        }

        [HttpPost]
        public IActionResult Payment(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payment = new Payment
                {
                    OrderId = model.OrderId,
                    PaymentMethod = model.PaymentMethod,
                    CardNumber = model.PaymentMethod == "Card" ? model.CardNumber : null,
                    UPIId = model.PaymentMethod == "UPI" ? model.UPIId : null,
                    PaymentDate = DateTime.Now
                };

                _context.Payments.Add(payment);

                // Mark order as Paid
                var order = _context.Orders.FirstOrDefault(o => o.Id == model.OrderId);
                if (order != null)
                {
                    order.Status = "Paid";
                }

                _context.SaveChanges();

                return RedirectToAction("PaymentSuccess", new { orderId = model.OrderId });
            }

            return View(model);
        }

        public IActionResult PaymentSuccess(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound(); // Optional safety check
            }

            ViewBag.PaymentMethod = _context.Payments
                .Where(p => p.OrderId == orderId)
                .Select(p => p.PaymentMethod)
                .FirstOrDefault();

            return View(order);
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoApp.Models;
using DemoApp.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DemoApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderContext _context;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;

        public OrdersController(OrderContext context, IConfiguration config, ITokenService tokenService, ILogger<OrdersController> logger)
        {
            _context = context;
            _tokenService = tokenService;
            _config = config;
            _logger = logger;
        }

        public OrdersController()
        {
        }

        // GET: Orders
        [Authorize]
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? pageNumber, string currentFilter)
        {
            try
            {
                string token = HttpContext.Session.GetString("Token");
                if (token == null)
                {
                    return (RedirectToAction("../Home/Index"));
                }
                if (!_tokenService.IsTokenValid(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), token))
                {
                    return (View("Home"));
                }

                ViewData["ProductNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "product_name_desc" : "";
                ViewData["CategoryNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "category_name_desc" : "";
                ViewData["CustomerNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "customer_name_desc" : "";
                ViewData["CurrentSort"] = sortOrder;

                if (searchString != null)
                {
                    pageNumber = 1;
                }
                else
                {
                    searchString = currentFilter;
                }

                var customer = _context.Set<Customer>();
                var order = _context.Set<Order>();
                var product = _context.Set<Product>();
                var category = _context.Set<Category>();

                var joinTable = order.Join(customer, o => o.CustomerID, cust => cust.CustomerID, (o, cust) => new { o, cust })
                                     .Join(product, t => t.o.ProductID, p => p.ProductID, (t, p) => new { t.o, t.cust, p })
                                     .Join(category, t => t.p.CategoryID, cat => cat.CategoryID, (t, cat) => new { t.cust, t.o, t.p, cat })
                                     ;
                var result = joinTable.Select(t => new OrderView
                {
                    ProductName = t.p.Name,
                    CategoryName = t.cat.Name,
                    CustomerName = t.cust.Name,
                    OrderDate = t.o.OrderDate,
                    Amount = t.o.Amount
                });

                if (!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(s => s.CategoryName.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "product_name_desc":
                        result = result.OrderByDescending(s => s.ProductName);
                        break;
                    case "category_name_desc":
                        result = result.OrderByDescending(s => s.CategoryName);
                        break;
                    case "customer_name_desc":
                        result = result.OrderByDescending(s => s.CustomerName);
                        break;
                    default:
                        result = result.OrderBy(s => s.ProductName);
                        break;
                }

                int pageSize = 3;
                return View(await PaginatedList<OrderView>.CreateAsync(result.AsNoTracking(), pageNumber ?? 1, pageSize));
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        // GET: Orders/Create
        public IActionResult AddOrEdit()
        {
            try
            {
                List<Customer> cust = _context.Set<Customer>().ToList();
                cust.Insert(0, new Customer { CustomerID = Guid.Empty, Name = "--Select Customer Name--" });

                List<Product> product = _context.Set<Product>().ToList();
                product.Insert(0, new Product { ProductID = Guid.Empty, Name = "--Select Product Name--" });

                ViewBag.Customer = cust;
                ViewBag.Product = product;
                return View(new Order());
            }catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(new Order());
            }
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("OrderID,CustomerID,ProductID,OrderName,Amount,OrderDate")] Order order)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    order.OrderID = Guid.NewGuid();
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(order);
            } catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View();
            }
        }

        private bool OrderExists(Guid id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}

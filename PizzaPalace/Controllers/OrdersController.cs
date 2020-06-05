using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PizzaPalace.Helpers;
using PizzaPalace.Models;
using PizzaPalace.ViewModels;

namespace PizzaPalace.Controllers
{
    public class OrdersController : Controller
    {
        private readonly PizzaContext _context;

        public OrdersController(PizzaContext context)
        {
            _context = context;
        }

        private Tuple<IList<Size>, IList<Topping>> GetData()
        {
            IList<Size> sizes = new List<Size>() {
                new Size(){ Id = 1, Name = "Small", Price = 12 },
                new Size(){ Id = 2, Name = "Medium", Price = 14 },
                new Size(){ Id = 3, Name = "Large", Price = 16 },
            };

            IList<Topping> toppings = new List<Topping>() {
                new Topping(){ Id = 1, Name = "Cheese", SmallPrice = 0.50M, MediumPrice = 0.75M, LargePrice = 1.00M },
                new Topping(){ Id = 2, Name = "Pepperoni", SmallPrice = 0.50M, MediumPrice = 0.75M, LargePrice = 1.00M },
                new Topping(){ Id = 3, Name = "Ham", SmallPrice = 0.50M, MediumPrice = 0.75M, LargePrice = 1.00M },
                new Topping(){ Id = 4, Name = "Pineapple", SmallPrice = 0.50M, MediumPrice = 0.75M, LargePrice = 1.00M },
                new Topping(){ Id = 5, Name = "Sausage", SmallPrice = 0.50M, MediumPrice = 0.75M, LargePrice = 1.00M },
                new Topping(){ Id = 6, Name = "Feta Cheese", SmallPrice = 0.50M, MediumPrice = 0.75M, LargePrice = 1.00M},
                new Topping(){ Id = 7, Name = "Tomatoes", SmallPrice = 0.50M, MediumPrice = 0.75M, LargePrice = 1.00M },
                new Topping(){ Id = 8, Name = "Olives", SmallPrice = 0.50M, MediumPrice = 0.75M, LargePrice = 1.00M },
            };

            var data = Tuple.Create(sizes, toppings);

            return data;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Order.ToListAsync());
        }

        // GET: Orders/Create
        // GET: Orders/Create/1
        [NoDirectAccess]
        public IActionResult Create(int id)
        {
            var data = GetData();

            if (id == 0)
            {
                var orderViewModel = new OrderViewModel
                {
                    Order = new Order(),
                    Sizes = data.Item1,
                    Toppings = data.Item2
                };
                return View(orderViewModel);
            }
            else
            {
                var order = _context.Order.Find(id);
                order.PizzaSizeList = JsonConvert.DeserializeObject<List<string>>(order.PizzaSize);
                order.ToppingsList = JsonConvert.DeserializeObject<List<string>>(order.Toppings);
                var orderViewModel = new OrderViewModel
                {
                    Order = order,
                    Sizes = data.Item1,
                    Toppings = data.Item2
                };
                return View(orderViewModel);
            }
        }

        // POST: Orders/Save/1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(int id, [Bind("OrderId,PizzaSize,PizzaQuantity,PizzaSizeList,Toppings,NumberOfToppings,ToppingsList,Subtotal,Tax,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.PizzaQuantity = order.PizzaSizeList.Count;
                order.NumberOfToppings = order.ToppingsList.Count;
                order.PizzaSize = JsonConvert.SerializeObject(order.PizzaSizeList);
                order.Toppings = JsonConvert.SerializeObject(order.ToppingsList);
                order.TotalPrice = Decimal.Round(CalculatePrice(order.PizzaSizeList, order), 2);
                if (id == 0)
                {
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        order.OrderId = id;
                        _context.Update(order);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!OrderExists(order.OrderId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Receipt", order) });
            }
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Create", order) });
        }

        // GET: Orders/Receipt
        [NoDirectAccess]
        public async Task<IActionResult> Receipt(int id)
        {
            var order = await _context.Order.FindAsync(id);
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Order.ToList()) });
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }

        /* Calculates the price for the order*/
        private decimal CalculatePrice(List<string> pizzaSizeList, Order model)
        {
            var data = GetData();
            decimal price = 0.00M;
            foreach (var pizzaSize in pizzaSizeList)
            {
                if (pizzaSize == "Small")
                {
                    price += data.Item1.Where(x => x.Name == pizzaSize).Select(y => y.Price).FirstOrDefault();
                    foreach (var item in model.ToppingsList)
                    {
                        foreach (var itemTwo in data.Item2.Where(x => x.Name == item))
                        {
                            price += itemTwo.SmallPrice;
                        }
                    }
                }
                else if (pizzaSize == "Medium")
                {
                    price += data.Item1.Where(x => x.Name == pizzaSize).Select(y => y.Price).FirstOrDefault();
                    foreach (var item in model.ToppingsList)
                    {
                        foreach (var itemTwo in data.Item2.Where(x => x.Name == item))
                        {
                            price += itemTwo.MediumPrice;
                        }
                    }
                }
                else if (pizzaSize == "Large")
                {
                    price += data.Item1.Where(x => x.Name == pizzaSize).Select(y => y.Price).FirstOrDefault();
                    foreach (var item in model.ToppingsList)
                    {
                        foreach (var itemTwo in data.Item2.Where(x => x.Name == item))
                        {
                            price += itemTwo.LargePrice;
                        }
                    }
                }
            }
            var GST = price * 0.05M;
            model.Subtotal = price;
            model.Tax = GST;

            return price + GST;
        }
    }


    public class Topping
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal SmallPrice { get; set; }
        public decimal MediumPrice { get; set; }
        public decimal LargePrice { get; set; }
        public string Display { get { return this.Name + " - " + "Small $" + this.SmallPrice + " " + "Medium $" + this.MediumPrice + " " + " Large $" + this.LargePrice; } }
    }

    public class Size
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Display { get { return this.Name + " - " + "$" + this.Price; } }
    }
}

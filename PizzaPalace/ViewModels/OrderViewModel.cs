using PizzaPalace.Controllers;
using PizzaPalace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalace.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<Size> Sizes { get; set; }
        public IEnumerable<Topping> Toppings { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaPalace.Models
{
    public class Order
    {
        [Display(Name = "Order Id")]
        public int OrderId { get; set; }

        [Display(Name = "Pizza Size")]
        public string PizzaSize { get; set; }

        [Display(Name = "Pizza Quantity")]
        public int PizzaQuantity { get; set; }

        [NotMapped]
        public List<string> PizzaSizeList { get; set; }

        public string Toppings { get; set; }

        public int NumberOfToppings { get; set; }

        [NotMapped]
        public List<string> ToppingsList { get; set; }

        [Display(Name = "Subtotal Price")]
        public decimal Subtotal { get; set; }

        public decimal Tax { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
    }
}

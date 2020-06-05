using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaPalace.Models;

namespace PizzaPalace.Models
{
    public class PizzaContext:DbContext
    {
        public PizzaContext(DbContextOptions<PizzaContext> options) : base(options)
        {
        }
        public DbSet<Order> Order { get; set; }
    }
}

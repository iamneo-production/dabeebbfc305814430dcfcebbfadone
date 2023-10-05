using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnetmicroserviceone.Models
{
    public class ProductDbContext: DbContext
    {
        public ProductDbContext(DbContextOptions<TaskContext> options)
        : base(options)
        {
        }
        public DbSet<Product> Products{get; set;}
    }
}
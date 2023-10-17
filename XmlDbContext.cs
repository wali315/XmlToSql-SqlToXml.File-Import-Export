using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class XmlDbContext : DbContext
    {
        public DbSet<XmlModel> XmlModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            MyConfiguration config = new MyConfiguration();
            var connection = config.GetConnectionString();
            optionsBuilder.UseSqlServer(connection);
        }
    }
}

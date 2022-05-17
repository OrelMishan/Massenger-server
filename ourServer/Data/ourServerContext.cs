using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ourServer.Models;

namespace ourServer.Data
{
    public class ourServerContext : DbContext
    {
        public ourServerContext (DbContextOptions<ourServerContext> options)
            : base(options)
        {
        }

        public DbSet<ourServer.Models.Rate>? Rate { get; set; }
    }
}

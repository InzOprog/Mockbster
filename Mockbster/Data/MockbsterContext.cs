using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mockbster.Models;

namespace Mockbster.Data
{
    public class MockbsterContext : DbContext
    {
        public MockbsterContext (DbContextOptions<MockbsterContext> options)
            : base(options)
        {
        }

        public DbSet<Mockbster.Models.MovieModel> Movie { get; set; } = default!;

        public DbSet<Mockbster.Models.UserModel> User { get; set; } = default!;

        public DbSet<Mockbster.Models.OrderModel> Order { get; set; } = default!;
    }
}

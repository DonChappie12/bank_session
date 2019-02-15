using System;
using Microsoft.EntityFrameworkCore;

namespace bank_session.Models
{
    public class UserContext : DbContext
    {
        public DbSet<User> user { get; set; }
        public DbSet<Transaction> transactions { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
    }
}
using AuthDemo2.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthDemo2.Contexts
{
    public class AuthPracticeDbContext : IdentityDbContext
    {
        private readonly DbContextOptions _options;

        public AuthPracticeDbContext(DbContextOptions options) : base(options) 
        {
            _options = options;
        }

        DbSet<Employee> Employees { get; set; }
    }
}

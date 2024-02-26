using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using todo_library.Models;

namespace todo_library.Data
{
    public class DataContext : IdentityDbContext
    {
        public DbSet<ToDo> ToDos { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
    }
}

namespace SignalRChat.Models
{
    using Microsoft.EntityFrameworkCore;

    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}
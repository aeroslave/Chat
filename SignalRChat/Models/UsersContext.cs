namespace SignalRChat.Models
{
    using Microsoft.EntityFrameworkCore;

    public sealed class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Message> Messages { get; set; }
    }
}
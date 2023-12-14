using IdentityProvider.Models.DomainModels;
using Microsoft.EntityFrameworkCore;

namespace IdentityProvider.Data;

public sealed class IdentityDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public IdentityDbContext(DbContextOptions<IdentityDbContext> dbContextOptions)
        : base(dbContextOptions)
    {
        Users = Set<User>();
        Database.EnsureCreated();
    }
}

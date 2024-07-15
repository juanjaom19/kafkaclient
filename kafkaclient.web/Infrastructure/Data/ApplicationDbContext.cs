using kafkaclient.web.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace kafkaclient.web.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Cluster> Clusters { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
using IceSync.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace IceSync.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Workflow> Workflows => Set<Workflow>();
}

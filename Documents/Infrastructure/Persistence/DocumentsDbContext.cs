using Documents;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Infrastructure.Persistence;

public class DocumentsDbContext : DbContext
{
    public DbSet<Document> Documents { get; set; }

    public DocumentsDbContext(DbContextOptions<DocumentsDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Document>().ToCollection("documents");
    }
}

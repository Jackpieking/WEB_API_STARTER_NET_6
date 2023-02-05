using Microsoft.EntityFrameworkCore;
using WEB_API.Entities;

namespace WEB_API;

public sealed class BookStoreContext : DbContext
{
    #region DbSet
    public DbSet<Book> Books { get; set; }
    #endregion

    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

    /**
     *
     * Configure entites
     *
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(buildAction: entityBuilder =>
        {
            entityBuilder.HasData(
                new Book()
                {
                    ID = 1,
                    Title = "Lesson 1",
                    Description = "Creating Web API",
                    Price = 10000,
                    Quantity = 1
                },
                new Book()
                {
                    ID = 2,
                    Title = "Lesson 2",
                    Description = "Creating Entity Book",
                    Price = 20000,
                    Quantity = 2
                });
        });
    }
}

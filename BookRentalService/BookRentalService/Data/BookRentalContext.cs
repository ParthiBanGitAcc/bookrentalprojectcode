using Microsoft.EntityFrameworkCore;
using BookRentalService.Models;

namespace BookRentalService.Data
{
    public class BookRentalContext : DbContext
    {
        public BookRentalContext(DbContextOptions<BookRentalContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rental> Rentals { get; set; }


    }
}

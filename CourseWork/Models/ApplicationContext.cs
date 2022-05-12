using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CourseWork.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Collection> Collections { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<AdditionalFieldName> AdditionalFieldNames { get; set; }
        public DbSet<AdditionalField> AdditionalFields { get; set; }
        public DbSet<CollectionElement> CollectionElements { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Theme>().HasData(
                new Theme[]
                {
                    new Theme { Id = 1, Name = "Alcohol" },
                    new Theme { Id = 2, Name = "Books" },
                    new Theme { Id = 3, Name = "Jewelry" },
                    new Theme { Id = 4, Name = "Phones" },
                    new Theme { Id = 5, Name = "Coins" },
                    new Theme { Id = 6, Name = "Gems" }
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
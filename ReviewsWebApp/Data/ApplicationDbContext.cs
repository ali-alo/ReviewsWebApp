﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReviewsWebApp.Models;

namespace ReviewsWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ReviewGroup> ReviewsGroup { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ReviewItem> ReviewsItems { get; set; }
        public DbSet<UserRatedReview> UserRatedReviews { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ReviewGroup>().HasIndex(rg => rg.NameEn).IsUnique();
            builder.Entity<ReviewGroup>().HasIndex(rg => rg.NameRu).IsUnique();
            builder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();

            // one user can leave only one review on a ReviewItem
            builder.Entity<Review>()
                .HasIndex(r => new { r.CreatorId, r.ReviewItemId })
                .IsUnique();

            builder.Entity<UserRatedReview>().
                HasIndex(r => new { r.UserId, r.ReviewId })
                .IsUnique();

            builder.Entity<ReviewItem>()
                .HasOne(r => r.ReviewGroup)
                .WithMany(rg => rg.ReviewItems)
                .HasForeignKey(r => r.ReviewGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.ApplicationUser)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.CreatorId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<ApplicationUser>()
               .HasMany(u => u.LikedReviews)
               .WithMany(r => r.UsersWhoLiked)
               .UsingEntity<Dictionary<string, object>>(
                   "UserLikedReviews",
                   j => j
                       .HasOne<Review>()
                       .WithMany()
                       .HasForeignKey("ReviewId")
                       .OnDelete(DeleteBehavior.Cascade),
                   j => j
                       .HasOne<ApplicationUser>()
                       .WithMany()
                       .HasForeignKey("UserId")
                       .OnDelete(DeleteBehavior.Cascade),
                   j =>
                   {
                       j.HasKey("UserId", "ReviewId");
                       j.ToTable("UserLikedReviews");
                   });

            builder.Entity<ReviewGroup>().HasData(
                new ReviewGroup[]
                {
                    new ReviewGroup {Id = 100, NameEn = "Movies", NameRu = "Кино"},
                    new ReviewGroup {Id = 101, NameEn = "Books", NameRu = "Книги"},
                    new ReviewGroup {Id = 201, NameEn = "Games", NameRu = "Игры"}
                });
        }
    }
}
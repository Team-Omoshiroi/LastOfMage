﻿using Microsoft.EntityFrameworkCore;

namespace AccountServer.DB
{
    public class AppDbContext : DbContext
    {
        public DbSet<AccountDb> Accounts { get; set; }

        public DbSet<ItemDb> Item { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<AccountDb>()
                    .HasIndex(a => a.AccountName)
                    .IsUnique();


            builder.Entity<ItemDb>()
                .HasIndex(i => i.ItemId)
                .IsUnique();
        }
    }
}

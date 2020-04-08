﻿using DataAbstract;
using Microsoft.EntityFrameworkCore;

namespace MuKai_Music.DataContext
{
    public class MusicContext : DbContext
    {
        public MusicContext(DbContextOptions<MusicContext> options) : base(options)
        {

        }
        public DbSet<MusicInfo> MusicInfos { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MusicInfo>()
                .ToTable("MusicInfo")
                .HasIndex(e =>
                    e.Ne_Id
                );
        }
    }
}

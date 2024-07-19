﻿using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        
        public DbSet<ProductEntity> Products { get; set; }

        public DbSet<UserEntity> Users { get; set; }
    }
}
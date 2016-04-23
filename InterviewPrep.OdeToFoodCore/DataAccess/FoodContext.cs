﻿using System;
using InterviewPrep.OdeToFoodCore.Entities;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InterviewPrep.OdeToFoodCore.DataAccess
{
    public class FoodContext : IdentityDbContext<User>
    {
        private readonly string _connectionString;
        private readonly ConnectionType _type;
        public DbSet<Restaurant> Restaurants { get; set; }

        public FoodContext()
        {
        }

        public FoodContext(string connectionString, ConnectionType type)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException("test error", nameof(connectionString));
            if (type == ConnectionType.None)
                throw new ArgumentException("Must provide valid connection type", nameof(type));
            _connectionString = connectionString;
            _type = type;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Restaurant>().ToTable("Restaurants");
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_type == ConnectionType.Sql)
                optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
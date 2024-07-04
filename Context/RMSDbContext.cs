﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeManagementSystem.Data;
using RecipeManagementSystem.Models;
using System.Reflection;

namespace RecipeManagementSystem.Context
{
    public class RMSDbContext : IdentityDbContext<ApplicationUser>
    {
        public RMSDbContext(DbContextOptions<RMSDbContext> options) : base(options)
        {
        }

        public RMSDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }

    }

}

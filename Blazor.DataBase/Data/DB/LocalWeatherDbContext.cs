/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Microsoft.EntityFrameworkCore;
using System;

namespace Blazor.Database.Data
{
    /// <summary>
    /// Specific <see cref="DbContext"/> implementation for the Weather Database
    /// EF only contains the Readonly DBSet operations through Database Views
    /// CRUD operations are run from Stored Procedures defined in a set of extensions in <see cref="DbContextExtensions"/>.
    /// </summary>
    public class LocalWeatherDbContext : DbContext
    {
        /// <summary>
        /// Tracking lifetime of contexts.
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// New Method - creates a guid in case we need to track it
        /// </summary>
        /// <param name="options"></param>
        public LocalWeatherDbContext(DbContextOptions<LocalWeatherDbContext> options)
            : base(options)
            => _id = Guid.NewGuid();

        /// <summary>
        /// DbSet for the <see cref="DbWeatherForecast"/> record
        /// </summary>
        public DbSet<WeatherForecast> WeatherForecast { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}

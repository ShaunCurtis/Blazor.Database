/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================

using Microsoft.EntityFrameworkCore;
using System;

namespace Blazor.Database.Data
{
    /// <summary>
    /// Specific <see cref="DbContext"/> implementation for the Weather Database
    /// EF only contains the Readonly DBSet operations through Database Views
    /// CRUD operations are run from Stored Procedures defined in a set of extensions in <see cref="DbContextExtensions"/>.
    /// </summary>
    public class MSSQLWeatherDbContext : DbContext
    {
        /// <summary>
        /// Tracking lifetime of contexts.
        /// </summary>
        private readonly Guid _id;

        /// <summary>
        /// New Method - creates a guid in case we need to track it
        /// </summary>
        /// <param name="options"></param>
        public MSSQLWeatherDbContext(DbContextOptions<MSSQLWeatherDbContext> options)
            : base(options)
            => _id = Guid.NewGuid();

        /// <summary>
        /// DbSet for the <see cref="DbWeatherForecast"/> record
        /// </summary>
        public DbSet<WeatherForecast> WeatherForecast { get; set; }
    }
}

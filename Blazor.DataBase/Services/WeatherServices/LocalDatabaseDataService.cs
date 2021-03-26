/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using Blazor.SPA.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blazor.Database.Services
{
    public class LocalDatabaseDataService :
        FactoryServerDataService<LocalWeatherDbContext>
    {

        public LocalDatabaseDataService(IConfiguration configuration, IDbContextFactory<LocalWeatherDbContext> dbContext) : base(configuration, dbContext)
        {
            // Debug.WriteLine($"==> New Instance {this.ToString()} ID:{this.ServiceID.ToString()} ");
        }

    }
}

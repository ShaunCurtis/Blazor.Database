/// =================================
/// Author: Shaun Curtis, Cold Elm
/// License: MIT
/// ==================================

using Blazor.Database.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blazor.Database.Services
{
    public class TestDatabaseDataService :
        FactoryServerInMemoryDataService<InMemoryWeatherDbContext>
    {

        public TestDatabaseDataService(IConfiguration configuration, IDbContextFactory<InMemoryWeatherDbContext> dbContext) : base(configuration, dbContext)
        {
            // Debug.WriteLine($"==> New Instance {this.ToString()} ID:{this.ServiceID.ToString()} ");
        }

    }
}

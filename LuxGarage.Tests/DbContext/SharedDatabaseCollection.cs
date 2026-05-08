using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Microsoft.EntityFrameworkCore;
using LuxGarage.API.Data; 
using Xunit;

namespace LuxGarage.Tests;

[CollectionDefinition("SharedDB")]
public class SharedDatabaseCollection : ICollectionFixture<SharedDatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply to be the place to apply [CollectionDefinition] to indicate that tests use the same DB.
}
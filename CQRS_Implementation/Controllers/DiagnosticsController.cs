using CQRS_Implementation.Infrastructure.Data.MariaDb;
using CQRS_Implementation.Infrastructure.Data.MongoDB;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Implementation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiagnosticsController(MariaDbContext mariaDbContext, IMongoDbContext mongoDbContext)
   : ControllerBase
{
   [HttpGet("ping")]
   public async Task<IActionResult> Ping()
   {
      var result = new
      {
         MariaDbPing = await CheckMariaDbConnection(),
         MongoDbPing = await CheckMongoDbConnection()
      };
      return Ok(result);
   }
   private async Task<bool> CheckMariaDbConnection()
   {
      try
      {
         await mariaDbContext.Database.CanConnectAsync();
         return true;
      }
      catch
      {
         return false;
      }
   }
        
   private async Task<bool> CheckMongoDbConnection()
   {
      try
      {
         var database = mongoDbContext.GetCollection<object>(mongoDbContext.Settings.CollectionNames.Users).Database;
         await database.ListCollectionNamesAsync();
         return true;
      }
      catch
      {
         return false;
      }
   }
}
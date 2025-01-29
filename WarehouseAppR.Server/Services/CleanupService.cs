
using Microsoft.EntityFrameworkCore;

namespace WarehouseAppR.Server.Services
{
    public class CleanupService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private const int CheckIntervalMinutes = 1;
        public CleanupService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<WarehouseDbContext>();
                        var cutoffTime = DateTime.Now.AddMinutes(-5);

                        var affectedRowsPs = dbContext.PendingSales.Where(e => e.DateAdded < cutoffTime);
                        var affectedRowsIds = (await affectedRowsPs.ToListAsync()).Select(ps => ps.PendingSaleId).ToList();
                        var affectedRowsPsp = await dbContext.PendingSaleProducts.Where(psp => affectedRowsIds.Contains(psp.PendingSaleId)).ExecuteDeleteAsync(stoppingToken);
                        await affectedRowsPs.ExecuteDeleteAsync(stoppingToken);
                    }
                }
                catch
                {
                    throw new Exception("Cleanup Service error");
                }
                await Task.Delay(TimeSpan.FromMinutes(CheckIntervalMinutes), stoppingToken);
            }
        }
    }
    
}

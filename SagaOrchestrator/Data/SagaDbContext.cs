using Microsoft.EntityFrameworkCore;
using SagaOrchestrator.Models;

namespace SagaOrchestrator.Data
{
    public class SagaDbContext : DbContext
    {
        public SagaDbContext(DbContextOptions<SagaDbContext> options)
            : base(options)
        {
        }

        public DbSet<SagaVente> SagaVentes { get; set; } = null!;
    }
}

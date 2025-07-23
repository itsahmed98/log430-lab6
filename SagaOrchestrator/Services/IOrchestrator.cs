using SagaOrchestrator.Models;

namespace SagaOrchestrator.Services
{
    public interface IOrchestrator
    {
        Task<SagaVente> HandleAsync(VenteSagaDto dto);

    }
}

using SagaOrchestrator.Models;

namespace SagaOrchestrator.Services
{
    public interface IOrchestrator
    {
        Task<SagaVente> EnregistrerVente(VenteSagaDto dto);

    }
}

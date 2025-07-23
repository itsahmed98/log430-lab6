using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SagaOrchestrator.Models;
using SagaOrchestrator.Services;
using System.Data;

namespace SagaOrchestrator.Controllers
{
    [ApiController]
    [Route("api/v1/sagas/ventes")]
    public class VenteSagaController : ControllerBase
    {
        private readonly IOrchestrator _orchestrator;
        public VenteSagaController(IOrchestrator orchestrator)
            => _orchestrator = orchestrator;

        [HttpPost]
        public async Task<IActionResult> StartSagaVente([FromBody] VenteSagaDto dto)
        {
            try
            {
                var result = await _orchestrator.HandleAsync(dto);
                return CreatedAtAction(null, new { id = result.SagaVenteId }, result);
            }
            catch (SagaException ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

}

namespace SagaOrchestrator.Models
{
    public class SagaVente
    {
        public Guid SagaVenteId { get; set; }
        public int MagasinId { get; set; }
        public EtatVenteSaga Etat { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

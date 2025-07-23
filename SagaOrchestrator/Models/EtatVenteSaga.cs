namespace SagaOrchestrator.Models
{
    public enum EtatVenteSaga
    {
        Init,
        ProduitValide,
        QuantiteStockValide,
        StockDeduit,
        VenteCréée,
        CommandeConfirmee,
        ErreurProduit,
        ErreurStock,
        ErreurVente,
        ErreurCompensation
    }
}

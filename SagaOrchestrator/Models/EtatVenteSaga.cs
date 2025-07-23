namespace SagaOrchestrator.Models
{
    public enum EtatVenteSaga
    {
        Init,
        ProduitValide,
        QuantiteStockValide,
        StockDeduit,
        VenteCréée,
        Terminée,
        ErreurProduit,
        ErreurStock,
        ErreurVente,
        ErreurCompensation
    }
}

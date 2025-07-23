namespace SagaOrchestrator.Models
{
    public class LigneVenteSagaDto
    {
        /// <summary>
        /// L'identifiant du produit vendu dans cette ligne.
        /// </summary>
        public int ProduitId { get; set; }

        /// <summary>
        /// La quantité de produit vendue dans cette ligne.
        /// </summary>
        public int Quantite { get; set; }

        /// <summary>
        /// Le prix unitaire du produit au moment de la vente.
        /// </summary>
        public decimal PrixUnitaire { get; set; }
    }
}

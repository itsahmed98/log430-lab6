namespace SagaOrchestrator.Models
{
    public class ProduitDto
    {
        /// <summary>
        /// L'identifiant unique du produit.
        /// </summary>
        public int ProduitId { get; set; }

        /// <summary>
        /// Le nom du produit.
        /// </summary>
        public string Nom { get; set; } = "";

        /// <summary>
        /// La catégorie du produit.
        /// </summary>
        public string Categorie { get; set; } = "";

        /// <summary>
        /// Le prix unitaire du produit.
        /// </summary>
        public decimal Prix { get; set; }
    }
}

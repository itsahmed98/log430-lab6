﻿using System.ComponentModel.DataAnnotations;

namespace VenteMcService.Models
{
    public class VenteDto
    {
        /// <summary>
        /// L'identifiant du magasin où la vente a eu lieu. Peut etre null si la vente est en ligne.
        /// </summary>
        public int? MagasinId { get; set; }

        /// <summary>
        /// L'identifiant du client associé à la vente. Peut etre null si la vente est fait en personne.
        /// </summary>
        public int? ClientId { get; set; }

        /// <summary>
        /// Indique si la vente a été effectuée en ligne ou en physique.
        /// </summary>
        public bool IsEnLigne { get; set; }

        /// <summary>
        /// La date et l'heure de la vente.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Liste des lignes de vente associées à cette vente.
        /// </summary>
        public List<LigneVenteDto> Lignes { get; set; } = new List<LigneVenteDto>();
    }
}

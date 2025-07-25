﻿using System.ComponentModel.DataAnnotations;

namespace AdministrationMcService.Models
{
    /// <summary>
    /// Représente les indicateurs de performance pour un magasin à une date donnée.
    /// </summary>
    public class Performance
    {
        [Key]
        public int PerformanceId { get; set; }

        /// <summary>
        /// Identifiant du magasin (correspond à l'ID utilisé par le microservice Magasin ou Ventes).
        /// </summary>
        public int MagasinId { get; set; }

        /// <summary>
        /// Date du snapshot de performance.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Chiffre d'affaires total ce jour-là.
        /// </summary>
        public decimal ChiffreAffaires { get; set; }

        /// <summary>
        /// Nombre total de ventes.
        /// </summary>
        public int NbVentes { get; set; }

        /// <summary>
        /// Nombre de produits en rupture de stock signalés.
        /// </summary>
        public int RupturesStock { get; set; }

        /// <summary>
        /// Nombre de produits en surstock signalés.
        /// </summary>
        public int Surstock { get; set; }
    }
}

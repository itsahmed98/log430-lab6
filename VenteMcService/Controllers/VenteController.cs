﻿using Microsoft.AspNetCore.Mvc;
using VenteMcService.Models;
using VenteMcService.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VenteMcService.Controllers
{
    [ApiController]
    [Route("api/v1/ventes")]
    public class VenteController : ControllerBase
    {
        private readonly IVenteService _venteService;
        private readonly ILogger<VenteController> _logger;

        public VenteController(ILogger<VenteController> logger, IVenteService venteService)
        {
            _venteService = venteService ?? throw new ArgumentNullException(nameof(venteService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Récupérer toutes les ventes.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Requête GET - Récupération de toutes les ventes.");
            var list = await _venteService.GetAllAsync();
            _logger.LogInformation("{Count} ventes récupérées.", list.Count);
            return Ok(list);
        }

        /// <summary>
        /// Recupere une vente avec son identifiant
        /// </summary>
        /// <param name="id">L'identifiant de la vente</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Requête GET - Vente ID {Id}", id);
            var v = await _venteService.GetByIdAsync(id);
            if (v == null)
            {
                _logger.LogWarning("Vente ID {Id} non trouvée.", id);
                return NotFound(new { message = $"Vente ID={id} non trouvé." });
            }
            _logger.LogInformation("Vente ID {Id} trouvée.", id);
            return Ok(v);
        }

        /// <summary>
        /// Rétourner les ventes associés à un magasin spécifique
        /// </summary>
        /// <param name="magasinId">L'identifiant du magasin</param>
        /// <returns></returns>
        [HttpGet("magasin/{magasinId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByMagasin(int magasinId)
        {
            _logger.LogInformation("Requête GET - Ventes pour magasin ID {MagasinId}", magasinId);
            var list = await _venteService.GetByMagasinAsync(magasinId);
            _logger.LogInformation("{Count} ventes trouvées pour magasin ID {MagasinId}", list.Count, magasinId);
            return Ok(list);
        }

        /// <summary>
        /// Créer une nouvelle vente.
        /// </summary>
        /// <param name="venteDto">La nouvelle vente à créer</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] VenteDto venteDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Modèle invalide pour la création de vente : {@Vente}", venteDto);
                return BadRequest(ModelState);
            }

            var vente = new Vente
            {
                MagasinId = venteDto.MagasinId,
                ClientId = venteDto.ClientId,
                IsEnLigne = venteDto.IsEnLigne,
                Date = venteDto.Date,
                Lignes = venteDto.Lignes.Select(l => new LigneVente
                {
                    ProduitId = l.ProduitId,
                    Quantite = l.Quantite,
                }).ToList()
            };

            try
            {
                _logger.LogInformation("Création d’une vente : {@Vente}", vente);
                var created = await _venteService.CreateAsync(vente);
                _logger.LogInformation("Vente créée avec ID {Id}", created.VenteId);
                return CreatedAtAction(nameof(GetById), new { id = created.VenteId }, created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la création de la vente.");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        /// <summary>
        /// Supprimer une vente par son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la vente à supprimer</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Suppression de la vente ID {Id}", id);
                await _venteService.DeleteAsync(id);
                _logger.LogInformation("Vente ID {Id} supprimée avec succès.", id);
                return NoContent();
            }
            catch (KeyNotFoundException knf)
            {
                _logger.LogWarning("Vente ID {Id} introuvable pour suppression.", id);
                return NotFound(new { message = knf.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la suppression de la vente.");
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}

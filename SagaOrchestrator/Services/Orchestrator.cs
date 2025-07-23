using MassTransit;
using SagaOrchestrator.Data;
using SagaOrchestrator.Models;
using System.Data;
using System.Text.Json;

namespace SagaOrchestrator.Services
{
    public class Orchestrator : IOrchestrator
    {
        private readonly SagaDbContext _db;
        private readonly HttpClient _httpCatalogue, _httpInventaire, _httpVente;
        public Orchestrator(
            SagaDbContext db,
            IHttpClientFactory factory
        )
        {
            _db = db;
            _httpCatalogue = factory.CreateClient("CatalogueMcService");
            _httpInventaire = factory.CreateClient("InventaireMcService");
            _httpVente = factory.CreateClient("VenteMcService");
        }

        public async Task<SagaVente> HandleAsync(VenteSagaDto dto)
        {
            // 1. Créer l’entrée saga
            var saga = new SagaVente
            {
                SagaVenteId = Guid.NewGuid(),
                MagasinId = (int)dto.MagasinId,
                Etat = EtatVenteSaga.Init,
                UpdatedAt = DateTime.UtcNow
            };
            _db.SagaVentes.Add(saga);
            await _db.SaveChangesAsync();

            // 2. Vérifier le stock pour chaque ligne
            try
            {
                await UpdateEtat(saga, EtatVenteSaga.QuantiteStockValide);
                foreach (var ligne in dto.Lignes)
                {
                    var quantiteStock = await _httpInventaire
                      .GetFromJsonAsync<int>($"{_httpInventaire.BaseAddress}/stocks/{dto.MagasinId}/{ligne.ProduitId}");

                    if (quantiteStock < ligne.Quantite)
                        throw new InvalidOperationException(
                          $"Stock insuffisant pour produit {ligne.ProduitId}");
                }
            }
            catch (Exception ex)
            {
                return await Fail(saga, EtatVenteSaga.ErreurStock, ex.Message);
            }

            // 2. Recuperer informations produit
            try
            {
                await UpdateEtat(saga, EtatVenteSaga.ProduitValide);
                foreach (var ligne in dto.Lignes)
                {
                    var produit = await _httpCatalogue.GetFromJsonAsync<ProduitDto>($"{_httpCatalogue.BaseAddress}/{ligne.ProduitId}");
                    if (produit == null)
                        throw new InvalidOperationException($"Produit {ligne.ProduitId} non trouvé.");
                    ligne.PrixUnitaire = produit.Prix * ligne.Quantite;
                }
            }
            catch (Exception ex)
            {
                return await Fail(saga, EtatVenteSaga.ErreurProduit, ex.Message);
            }

            // 4. Creer la vente
            int venteId;
            try
            {
                await UpdateEtat(saga, EtatVenteSaga.VenteCréée);
                var venteResp = await _httpVente.PostAsJsonAsync("", dto);
                venteResp.EnsureSuccessStatusCode();

                var created = await venteResp.Content.ReadFromJsonAsync<JsonElement>();
                venteId = created.GetProperty("venteId").GetInt32();
            }
            catch (Exception ex)
            {
                return await Fail(saga, EtatVenteSaga.ErreurVente, ex.Message);
            }

            // 5.Mettre à jour le stock
            try
            {
                await UpdateEtat(saga, EtatVenteSaga.StockDeduit);
                foreach (var ligne in dto.Lignes)
                {
                    var updateResp = await _httpInventaire.PutAsync($"{_httpInventaire.BaseAddress}/stocks?magasinId={dto.MagasinId}&produitId={ligne.ProduitId}&quantite={-ligne.Quantite}", null);
                    updateResp.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                // si échec, annuler la vente côté microservice
                await _httpVente.DeleteAsync($"/api/v1/ventes/{venteId}");
                return await Fail(saga, EtatVenteSaga.ErreurCompensation, ex.Message);
            }

            // 6. Terminer
            await UpdateEtat(saga, EtatVenteSaga.Terminée);
            return saga;
        }

        private async Task UpdateEtat(SagaVente saga, EtatVenteSaga etat)
        {
            saga.Etat = etat;
            saga.UpdatedAt = DateTime.UtcNow;
            _db.SagaVentes.Update(saga);
            await _db.SaveChangesAsync();
        }

        private async Task<SagaVente> Fail(SagaVente saga, EtatVenteSaga etat, string msg)
        {
            saga.Etat = etat;
            saga.ErrorMessage = msg;
            saga.UpdatedAt = DateTime.UtcNow;
            _db.SagaVentes.Update(saga);
            await _db.SaveChangesAsync();
            throw new SagaException(msg, typeof(SagaVente), saga.SagaVenteId);
        }
    }
}

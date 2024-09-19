using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using UnpaidpostpaidBills;

[Route("api/[controller]")]
[ApiController]
public class BillsController : ControllerBase
{

    private readonly AppDbContext _dbContext;

    public BillsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/Bills
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacturationEncaissement>>> Get()
    {
        try
        {
            var data = await _dbContext.Bill
                .OrderByDescending(b => b.DATE)
                .Take(10)
                .AsNoTracking()
                .ToListAsync();

            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de la récupération des données : {ex.Message}");
        }
    }

    /*Liste des impayés par Client et par cycle à partir de 2019*/

    // GET: api/Bills/client/{clientName}/cycle/{cycle}/impaye-a-partir-de-2019
    [HttpGet("client/{clientName}/cycle/{cycle}/impaye-a-partir-de-2019")]
    public async Task<ActionResult<IEnumerable<FacturationEncaissement>>> GetSoldeAPartirDe2019ParClientEtCycle(string clientName, string cycle)
    {
        try
        {
            // Récupérer les soldes à partir de 2019 pour le client et le cycle spécifiés
            var soldeAPartirDe2019 = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower()
                            && f.CYCLE.ToLower() == cycle.ToLower()
                            && f.DATE.Year >= 2019
                            && f.SOLDE > 4)
                .OrderBy(f => f.PERIODE) // Order by PERIODE ascending
                .ToListAsync();

            return Ok(soldeAPartirDe2019);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors de la récupération des soldes à partir de 2019 pour le client et le cycle spécifiés : {ex.Message}");
        }
    }

    /*Montant total des impayés par Client et par Cycle anterieur à 2019*/

    // GET: api/Bills/client/{clientName}/cycle/{cycle}/sumsolde-anterieur-2019
    [HttpGet("client/{clientName}/cycle/{cycle}/sumsolde-anterieur-2019")]
    public async Task<ActionResult<BigInteger>> GetSumSoldeAnterieur2019ParClientEtCycle(string clientName, string cycle)
    {
        try
        {
            // Calculer la somme des soldes antérieurs à 2019 pour le client spécifié
            var sumSoldeAnterieur2019 = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower()
                            && f.CYCLE.ToLower() == cycle.ToLower()
                            && f.DATE.Year < 2019)
                .SumAsync(f => f.SOLDE);

            return Ok(sumSoldeAnterieur2019);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors du calcul de la somme des soldes antérieurs à 2019 : {ex.Message}");
        }
    }

    /*Montant total des impayés par Client et par cycle à partir de 2019*/

    // GET: api/Bills/client/{clientName}/cycle/{cycle}/sumsolde-a-partir-de-2019
    [HttpGet("client/{clientName}/cycle/{cycle}/sumsolde-a-partir-de-2019")]
    public async Task<ActionResult<BigInteger>> GetSumSoldeAPartirDe2019ParClientEtCycle(string clientName, string cycle)
    {
        try
        {
            // Calculer la somme des soldes à partir de 2019 pour le client spécifié
            var sumSoldeAPartirDe2019 = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower()
                            && f.CYCLE.ToLower() == cycle.ToLower()
                            && f.DATE.Year >= 2019)
                .SumAsync(f => f.SOLDE);

            return Ok(sumSoldeAPartirDe2019);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors du calcul de la somme des soldes à partir de 2019 : {ex.Message}");
        }
    }

    // GET: api/Bills/client/{clientName}/impaye-a-partir-de-2019
    [HttpGet("client/{clientName}/impaye-a-partir-de-2019")]
    public async Task<ActionResult<IEnumerable<FacturationEncaissement>>> GetByClientAPartirDe2019(string clientName)
    {
        try
        {
            // Filtrer les données par le nom du client (case-insensitive)
            var data = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower() && f.DATE.Year >= 2019 && f.SOLDE > 4)
                .OrderByDescending(f => f.DATE)
                .ToListAsync();

            if (data == null || data.Count == 0)
            {
                return NotFound($"Aucune donnée trouvée pour le client '{clientName}'.");
            }

            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors de la récupération des données : {ex.Message}");
        }
    }

    // GET: api/Bills/client/{clientName}
    [HttpGet("client/{clientName}")]
    public async Task<ActionResult<IEnumerable<FacturationEncaissement>>> GetByClient(string clientName)
    {
        try
        {
            // Filtrer les données par le nom du client (case-insensitive)
            var data = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower() && f.SOLDE > 4)
                .OrderByDescending(f => f.DATE)
                .ToListAsync();

            if (data == null || data.Count == 0)
            {
                return NotFound($"Aucune donnée trouvée pour le client '{clientName}'.");
            }

            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors de la récupération des données : {ex.Message}");
        }
    }

    /*Montant total des impayés par Client anterieur à 2019*/

    // GET: api/Bills/client/{clientName}/sumsolde-anterieur-2019
    [HttpGet("client/{clientName}/sumsolde-anterieur-2019")]
    public async Task<ActionResult<BigInteger>> GetSumSoldeAnterieur2019(string clientName)
    {
        try
        {
            // Calculer la somme des soldes antérieurs à 2019 pour le client spécifié
            var sumSoldeAnterieur2019 = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower() && f.DATE.Year < 2019)
                .SumAsync(f => f.SOLDE);

            return Ok(sumSoldeAnterieur2019);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors du calcul de la somme des soldes antérieurs à 2019 : {ex.Message}");
        }
    }

    /*Montant total des impayés par Client à partir de 2019*/

    // GET: api/Bills/client/{clientName}/sumsolde-a-partir-de-2019
    [HttpGet("client/{clientName}/sumsolde-a-partir-de-2019")]
    public async Task<ActionResult<BigInteger>> GetSumSoldeAPartirDe2019(string clientName)
    {
        try
        {
            // Calculer la somme des soldes à partir de 2019 pour le client spécifié
            var sumSoldeAPartirDe2019 = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower() && f.DATE.Year >= 2019)
                .SumAsync(f => f.SOLDE);

            return Ok(sumSoldeAPartirDe2019);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors du calcul de la somme des soldes à partir de 2019 : {ex.Message}");
        }
    }

    // GET: api/Bills/BillExists/{value}
    [HttpGet("BillExists/{value}")]
    public async Task<ActionResult<bool>> BillExists(string value)
    {
        try
        {
            // Vérifier si des enregistrements existent pour le client spécifié avec SOLDE > 4
            var exists = await _dbContext.Bill
                .AnyAsync(d => d.CLIENT.ToLower() == value.ToLower() && d.SOLDE > 4); // Modifier la condition selon votre vue et la propriété à vérifier

            return Ok(exists);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de la vérification de l'existence des données : {ex.Message}");
        }
    }


    /*Montant total des factures par Client*/

    // GET: api/Bills/client/{clientName}/sumbill
    [HttpGet("client/{clientName}/sumbill")]
    public async Task<ActionResult<BigInteger>> GetSumBill(string clientName)
    {
        try
        {
            // Calculer la somme des montants de facturation pour le client spécifié
            var sumBill = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower())
                .SumAsync(f => f.MONTANT_FACT);

            return Ok(sumBill);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors du calcul de la somme des montants de facturation : {ex.Message}");
        }
    }

    /*Montant total des impayés par Client*/

    // GET: api/Bills/client/{clientName}/sumsolde
    [HttpGet("client/{clientName}/sumsolde")]
    public async Task<ActionResult<BigInteger>> GetSumSolde(string clientName)
    {
        try
        {
            // Calculer la somme des soldes pour le client spécifié
            var sumSolde = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower())
                .SumAsync(f => f.SOLDE);

            return Ok(sumSolde);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors du calcul de la somme des soldes : {ex.Message}");
        }
    }

    /*Montant total des règlements par Client*/

    // GET: api/Bills/client/{clientName}/summontantregle
    [HttpGet("client/{clientName}/summontantregle")]
    public async Task<ActionResult<BigInteger>> GetSumMontantRegle(string clientName)
    {
        try
        {
            // Calculer la somme des montants réglés pour le client spécifié
            var sumMontantRegle = await _dbContext.Bill
                .Where(f => f.CLIENT.ToLower() == clientName.ToLower())
                .SumAsync(f => f.MONTANT_REGLE);

            return Ok(sumMontantRegle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erreur lors du calcul de la somme des montants réglés : {ex.Message}");
        }
    }

    /*
    // GET: api/Bills/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Bill>> GetById(int id)
    {
        try
        {
            var data = await _dbContext.Bill.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de la récupération de la donnée : {ex.Message}");
        }
    }

    // POST: api/Bills
    [HttpPost]
    public async Task<ActionResult<Bill>> Post([FromBody] Bill newData)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _dbContext.Bill.Add(newData);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = newData.Id }, newData);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de l'ajout de la donnée : {ex.Message}");
        }
    }

    // PUT: api/Bills/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Bill>> Put(int id, [FromBody] Bill updatedData)
    {
        if (id != updatedData.Id)
        {
            return BadRequest("L'ID de la donnée à mettre à jour ne correspond pas à l'ID spécifié dans la requête.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            _dbContext.Entry(updatedData).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return Ok(updatedData);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DataExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de la mise à jour de la donnée : {ex.Message}");
        }
    }

    // DELETE: api/Bills/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var dataToDelete = await _dbContext.Bill.FindAsync(id);
            if (dataToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Bill.Remove(dataToDelete);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de la suppression de la donnée : {ex.Message}");
        }
    }

    private bool DataExists(int id)
    {
        return _dbContext.Bill.Any(e => e.Id == id);
    }*/
}

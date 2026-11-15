using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using SumaqAgro.Platform.Farming.Domain.Model.Aggregates;
using SumaqAgro.Platform.Iam.Interfaces.Acl;
using SumaqAgro.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

namespace SumaqAgro.Platform.Farming.Interfaces.Rest;

[ApiController]
[Route("api/v1/campaign-ledgers")]
public sealed class CampaignLedgersController(AppDbContext db, IIamAccounts accounts) : ControllerBase
{
    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    [HttpGet]
    public async Task<IActionResult> List([FromQuery, Range(1, long.MaxValue)] long campaignId, CancellationToken ct)
    {
        if (!await OwnsCampaignAsync(campaignId, ct)) return NotFound();
        var ledger = await db.Set<CampaignLedger>().FindAsync([campaignId], ct);
        return Ok(ledger is null ? Array.Empty<LedgerResource>() : new[] { Read(ledger) });
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken ct)
    {
        if (!await OwnsCampaignAsync(id, ct)) return NotFound();
        var ledger = await db.Set<CampaignLedger>().FindAsync([id], ct);
        return ledger is null ? NotFound() : Ok(Read(ledger));
    }

    [HttpPost]
    public async Task<IActionResult> Create(LedgerResource resource, CancellationToken ct)
    {
        if (!await OwnsCampaignAsync(resource.CampaignId, ct)) return NotFound();
        var existing = await db.Set<CampaignLedger>().FindAsync([resource.CampaignId], ct);
        if (existing is not null) return Ok(Read(existing));

        var ledger = new CampaignLedger(resource.CampaignId);
        ledger.Save(JsonSerializer.Serialize(resource with { Id = resource.CampaignId }, Json));
        db.Add(ledger);
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateException ex) when (ex.InnerException is MySqlException { Number: 1062 })
        {
            db.Entry(ledger).State = EntityState.Detached;
            var winner = await db.Set<CampaignLedger>().FindAsync([resource.CampaignId], ct);
            return winner is null ? Conflict() : Ok(Read(winner));
        }
        return CreatedAtAction(nameof(Get), new { id = resource.CampaignId }, Read(ledger));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, LedgerResource resource, CancellationToken ct)
    {
        if (id != resource.CampaignId) return BadRequest();
        if (!await OwnsCampaignAsync(id, ct)) return NotFound();
        var ledger = await db.Set<CampaignLedger>().FindAsync([id], ct);
        if (ledger is null) return NotFound();
        if (Read(ledger).Frozen) return Conflict();
        ledger.Save(JsonSerializer.Serialize(resource with { Id = id }, Json));
        try { await db.SaveChangesAsync(ct); }
        catch (DbUpdateConcurrencyException) { return Conflict(); }
        return Ok(Read(ledger));
    }

    private async Task<bool> OwnsCampaignAsync(long id, CancellationToken ct)
    {
        var email = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        if (string.IsNullOrWhiteSpace(email) || await accounts.FindAsync(email, ct) is not { Role: "ROLE_FARMER" } account)
            return false;

        return await (from campaign in db.Set<CropCampaign>()
                      join plot in db.Set<FieldPlot>() on campaign.FieldPlotId equals plot.Id
                      where campaign.Id == id && plot.OwnerUserId == account.Id
                      select campaign.Id).AnyAsync(ct);
    }

    private static LedgerResource Read(CampaignLedger ledger) =>
        JsonSerializer.Deserialize<LedgerResource>(ledger.Payload, Json)!;
}

public sealed record LedgerResource(long Id, [param: Range(1, long.MaxValue)] long CampaignId,
    [param: Range(0, double.MaxValue)] decimal ExpectedYield, [param: Range(0, double.MaxValue)] decimal? ActualYield,
    [param: Required, RegularExpression("^(SACK|QUINTAL)$")] string YieldUnit,
    bool Frozen, [param: Required] LedgerExpenseResource[] Entries);

public sealed record LedgerExpenseResource(long Id, [param: Required, RegularExpression("^(INPUTS|LABOR|FREIGHT)$")] string Category,
    [param: Required, StringLength(250)] string Description, [param: Range(0.000001, double.MaxValue)] decimal Quantity,
    [param: Range(0, double.MaxValue)] decimal UnitPrice, DateOnly ExpenseDate, string? Notes, string? ClientSyncId);

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;
using BellaFrisoer.Infrastructure.Data;

namespace BellaFrisoer.Infrastructure.Repositories
{
    public class TreatmentRepository : ITreatmentRepository
    {
        private readonly IDbContextFactory<BellaFrisoerWebUiContext> _dbFactory;

        public TreatmentRepository(IDbContextFactory<BellaFrisoerWebUiContext> dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public async Task<IReadOnlyList<Treatment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await ctx.Treatments
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<Treatment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            return await ctx.Treatments
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default)
        {
            if (treatment is null) throw new ArgumentNullException(nameof(treatment));
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Treatments.Add(treatment);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default)
        {
            if (treatment is null) throw new ArgumentNullException(nameof(treatment));
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            ctx.Treatments.Update(treatment);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);
            var entity = await ctx.Treatments.FindAsync(new object[] { id }, cancellationToken);
            if (entity is null) return;
            ctx.Treatments.Remove(entity);
            await ctx.SaveChangesAsync(cancellationToken);
        }
        public async Task<List<Treatment>> FilterTreatmentsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync(cancellationToken);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await ctx.Treatments
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }

            searchTerm = searchTerm.Trim().ToLower();

            return await ctx.Treatments
                .AsNoTracking()
                .Where(t =>
                    t.Name.ToLower().Contains(searchTerm) ||
                    t.Price.ToString().Contains(searchTerm) ||
                    t.Duration.ToString().Contains(searchTerm)
                )
                .ToListAsync(cancellationToken);
        }

    }
}
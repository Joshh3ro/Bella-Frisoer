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
        private readonly BellaFrisoerWebUiContext _context;

        public TreatmentRepository(BellaFrisoerWebUiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IReadOnlyList<Treatment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Treatments
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task<Treatment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Treatments
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task AddAsync(Treatment treatment, CancellationToken cancellationToken = default)
        {
            if (treatment is null)
                throw new ArgumentNullException(nameof(treatment));

            _context.Treatments.Add(treatment);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default)
        {
            if (treatment is null)
                throw new ArgumentNullException(nameof(treatment));
            try
            {
                _context.Attach(treatment);
                _context.Entry(treatment).State = EntityState.Modified;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ApplicationException(
                    "Behandlingen kunne ikke opdateres, da den blev ændret af en anden bruger. Prøv venligst at opdatere siden og prøv igen.",
                    ex);
            }
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Treatments.FindAsync(new object[] { id }, cancellationToken);

            if (entity is null)
                return;

            try
            {
                _context.Treatments.Remove(entity);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ApplicationException(
                    "Behandlingen kunne ikke slettes, da den blev ændret af en anden bruger. Prøv venligst at opdatere siden og prøv igen.",
                    ex);
            }
        }

        public async Task<List<Treatment>> FilterTreatmentsAsync(string searchTerm, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return await _context.Treatments
                    .AsNoTracking()
                    .OrderBy(t => t.Name)
                    .ToListAsync(cancellationToken);
            }

            searchTerm = searchTerm.Trim().ToLower();

            return await _context.Treatments
                .AsNoTracking()
                .Where(t =>
                    t.Name.ToLower().Contains(searchTerm) ||
                    t.Price.ToString().Contains(searchTerm) ||
                    t.Duration.ToString().Contains(searchTerm)
                )
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
    }
}

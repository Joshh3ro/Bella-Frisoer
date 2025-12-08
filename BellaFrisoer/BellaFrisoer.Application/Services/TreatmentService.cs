using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BellaFrisoer.Application.Interfaces;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Services
{
    public class TreatmentService : ITreatmentService
    {
        private readonly ITreatmentRepository _repository;

        public TreatmentService(ITreatmentRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<bool> CanCreateTreatmentAsync(Treatment newTreatment)
        {
            if (newTreatment is null) throw new ArgumentNullException(nameof(newTreatment));

            // Basic business rules: must have a name and positive duration
            if (string.IsNullOrWhiteSpace(newTreatment.Name) || newTreatment.DurationMinutes <= 0)
                return false;

            // Ensure product number uniqueness (simple example)
            var all = await _repository.GetAllAsync();
            var list = (all as List<Treatment>) ?? all.ToList();
            return !list.Any(t => string.Equals(t.ProductNumber?.Trim(), newTreatment.ProductNumber?.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        public async Task AddTreatmentAsync(Treatment treatment)
        {
            if (treatment is null) throw new ArgumentNullException(nameof(treatment));
            await _repository.AddAsync(treatment);
        }

        public async Task<List<Treatment>> GetAllAsync()
        {
            var all = await _repository.GetAllAsync();
            return (all as List<Treatment>) ?? all.ToList();
        }

        // New: retrieve single treatment by id
        public async Task<Treatment?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // New: update treatment
        public async Task UpdateTreatmentAsync(Treatment treatment)
        {
            if (treatment is null) throw new ArgumentNullException(nameof(treatment));
            await _repository.UpdateAsync(treatment);
        }

        public async Task DeleteTreatmentAsync(Treatment treatment)
        {
            if (treatment is null) throw new ArgumentNullException(nameof(treatment));
            await _repository.DeleteAsync(treatment.Id);
        }
    }
}
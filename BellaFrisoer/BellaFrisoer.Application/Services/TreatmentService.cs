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
            if (newTreatment is null)
                throw new ArgumentNullException(nameof(newTreatment));
            if (string.IsNullOrWhiteSpace(newTreatment.Name) || newTreatment.Duration <= 0)
                return false;
            var all = await _repository.GetAllAsync();
            return !all.Any(t => string.Equals(newTreatment.Id, StringComparison.OrdinalIgnoreCase));
        }

        public async Task AddTreatmentAsync(Treatment treatment)
        {
            if (treatment is null)
                throw new ArgumentNullException(nameof(treatment));
            await _repository.AddAsync(treatment);
        }

        public async Task<List<Treatment>> GetAllAsync()
        {
            var all = await _repository.GetAllAsync();
            return all.ToList();
        }

        public async Task<Treatment?> GetTreatmentByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task DeleteTreatmentAsync(Treatment treatment)
        {
            if (treatment is null)
                throw new ArgumentNullException(nameof(treatment));
            await _repository.DeleteAsync(treatment.Id);
        }

        public async Task<Treatment?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task UpdateTreatmentAsync(Treatment treatment)
        {
            if (treatment is null) throw new ArgumentNullException(nameof(treatment));
            await _repository.UpdateAsync(treatment);
        }
        public async Task<List<Treatment>> FilterTreatmentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllAsync();

            var filtered = await _repository.FilterTreatmentsAsync(searchTerm);
            return filtered.ToList();
        }
    }
}
using System.Collections.Generic;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    public interface ITreatmentService
    {
        Task<Treatment?> GetTreatmentByIdAsync(int id);
        Task DeleteTreatmentAsync(Treatment treatment);
        Task AddTreatmentAsync(Treatment treatment);
        Task<List<Treatment>> GetAllAsync();

        // Added for read/update flow from UI
        Task<Treatment?> GetByIdAsync(int id);
        Task UpdateTreatmentAsync(Treatment treatment);

        Task DeleteTreatmentAsync(Treatment treatment);
    }
}
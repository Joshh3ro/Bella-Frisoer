using System.Collections.Generic;
using System.Threading.Tasks;
using BellaFrisoer.Domain.Models;

namespace BellaFrisoer.Application.Interfaces
{
    public interface ITreatmentService
    {
        Task<bool> CanCreateTreatmentAsync(Treatment newTreatment);
        Task AddTreatmentAsync(Treatment treatment);
        Task<List<Treatment>> GetAllAsync();
    }
}
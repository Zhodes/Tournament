using AutoMapper;
using Domain.Contracts.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.DTOs.Tournaments;

namespace Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TournamentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task<TournamentDto> CreateAsync(TournamentForCreationDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TournamentDto>> GetAllAsync(bool includeGames, string? sortBy, string? order)
        {
            throw new NotImplementedException();
        }

        public Task<TournamentDto?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<TournamentDto?> PatchAsync(int id, JsonPatchDocument<TournamentDto> patchDoc)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(int id, TournamentForUpdateDto dto)
        {
            throw new NotImplementedException();
        }
        // Implement methods for tournament management here
    }
    
    
}

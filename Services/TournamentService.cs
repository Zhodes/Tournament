using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Tournament.DTOs.Shared;
using Tournament.DTOs.Tournaments;

namespace Tournament.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TournamentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<TournamentDto>> GetAllAsync(
    bool includeGames, string? sortBy, string? order, int page, int pageSize)
        {
            const int maxPageSize = 100;
            pageSize = Math.Min(pageSize <= 0 ? 20 : pageSize, maxPageSize);
            page = page <= 0 ? 1 : page;

            IEnumerable<TournamentDetails> tournaments = includeGames
                ? await _unitOfWork.TournamentRepository.GetAllIncludingGamesAsync()
                : await _unitOfWork.TournamentRepository.GetAllAsync();

            // Sorting
            tournaments = sortBy?.ToLower() switch
            {
                "title" => order == "desc" ? tournaments.OrderByDescending(t => t.Title) : tournaments.OrderBy(t => t.Title),
                "startdate" => order == "desc" ? tournaments.OrderByDescending(t => t.StartDate) : tournaments.OrderBy(t => t.StartDate),
                _ => tournaments
            };

            var totalItems = tournaments.Count();
            var pagedItems = tournaments
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var dtos = _mapper.Map<IEnumerable<TournamentDto>>(pagedItems);

            return new PagedResult<TournamentDto>
            {
                Items = dtos,
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }


        public async Task<TournamentDto?> GetByIdAsync(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetByIdAsync(id);
            return tournament == null ? null : _mapper.Map<TournamentDto>(tournament);
        }

        public async Task<TournamentDto> CreateAsync(TournamentForCreationDto dto)
        {
            var entity = _mapper.Map<TournamentDetails>(dto);

            foreach (var game in entity.Games)
            {
                game.TournamentDetails = entity;
            }

            _unitOfWork.TournamentRepository.Add(entity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<TournamentDto>(entity);
        }

        public async Task<bool> UpdateAsync(int id, TournamentForUpdateDto dto)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetByIdAsync(id);
            if (tournament == null) return false;

            tournament.Title = dto.Title;
            tournament.StartDate = dto.StartDate;

            tournament.Games.Clear();
            foreach (var gameId in dto.GameIds)
            {
                var game = await _unitOfWork.GameRepository.GetByIdAsync(gameId);
                if (game == null) throw new InvalidOperationException($"Game ID {gameId} not found.");
                tournament.Games.Add(game);
            }

            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetByIdAsync(id);
            if (tournament == null) return false;

            _unitOfWork.TournamentRepository.Remove(tournament);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<TournamentDto?> PatchAsync(int id, JsonPatchDocument<TournamentDto> patchDoc)
        {
            var tournament = await _unitOfWork.TournamentRepository.GetByIdAsync(id);
            if (tournament == null) return null;

            var dto = _mapper.Map<TournamentDto>(tournament);
            patchDoc.ApplyTo(dto);
            _mapper.Map(dto, tournament);

            await _unitOfWork.CompleteAsync();
            return dto;
        }
    }
}

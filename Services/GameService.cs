using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Tournament.DTOs.Games;
using Tournament.DTOs.Shared;

namespace Tournament.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResult<GameDto>> GetAllGamesAsync(int page, int pageSize, string? sortBy, string? order)
        {
            var games = await _unitOfWork.GameRepository.GetAllAsync();

            // Sorting
            games = sortBy?.ToLower() switch
            {
                "title" => order == "desc" ? games.OrderByDescending(g => g.Title) : games.OrderBy(g => g.Title),
                "time" => order == "desc" ? games.OrderByDescending(g => g.Time) : games.OrderBy(g => g.Time),
                _ => games
            };

            // Pagination rules
            const int maxPageSize = 100;
            pageSize = Math.Min(pageSize > 0 ? pageSize : 20, maxPageSize);
            page = page <= 0 ? 1 : page;

            var totalItems = games.Count();
            var pagedGames = games
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<GameDto>
            {
                Items = _mapper.Map<IEnumerable<GameDto>>(pagedGames),
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = page
            };
        }


        public async Task<ActionResult<GameDto>> GetGameByTitleAsync(string title)
        {
            var game = await _unitOfWork.GameRepository.GetByTitleAsync(title);
            if (game == null)
                return new NotFoundObjectResult($"Game with Title {title} not found.");
            return new OkObjectResult(_mapper.Map<GameDto>(game));
        }

        public async Task<IActionResult> UpdateGameAsync(int id, GameForUpdateDto gameDto)
        {
            if (id != gameDto.Id)
                return new BadRequestObjectResult("ID mismatch.");

            var existingGame = await _unitOfWork.GameRepository.GetByIdAsync(id);
            if (existingGame == null)
                return new NotFoundObjectResult($"Game with ID {id} not found.");

            _mapper.Map(gameDto, existingGame);
            await _unitOfWork.CompleteAsync();
            return new NoContentResult();
        }

        public async Task<GameDto> CreateGameAsync(GameForCreationDto gameDto)
        {
            var gameEntity = _mapper.Map<Game>(gameDto);
            _unitOfWork.GameRepository.Add(gameEntity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<GameDto>(gameEntity);
        }

        public async Task<IActionResult> DeleteGameAsync(int id)
        {
            var game = await _unitOfWork.GameRepository.GetByIdAsync(id);
            if (game == null)
                return new NotFoundResult();

            _unitOfWork.GameRepository.Remove(game);
            await _unitOfWork.CompleteAsync();
            return new NoContentResult();
        }
        public async Task<ActionResult<GameDto>> PatchGameAsync(int id, JsonPatchDocument<GameDto> patchDoc, ModelStateDictionary modelState)
        {
            if (patchDoc == null)
                return new BadRequestObjectResult("Patch document cannot be null.");

            var game = await _unitOfWork.GameRepository.GetByIdAsync(id);
            if (game == null)
                return new NotFoundObjectResult($"Game with ID {id} not found.");

            var gameDto = _mapper.Map<GameDto>(game);

            patchDoc.ApplyTo(gameDto, error =>
            {
                modelState.AddModelError(error.AffectedObject?.ToString() ?? "", error.ErrorMessage);
            });

            if (!modelState.IsValid)
                return new BadRequestObjectResult(modelState);

            _mapper.Map(gameDto, game);
            await _unitOfWork.CompleteAsync();

            return new OkObjectResult(gameDto);
        }
    }
}

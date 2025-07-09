using AutoMapper;
using Domain.Contracts.Repositories;
using Tournament.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<IGameService> _gameService;
    private readonly Lazy<ITournamentService> _tournamentService;

    public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _gameService = new Lazy<IGameService>(() => new GameService(unitOfWork, mapper));
        _tournamentService = new Lazy<ITournamentService>(() => new TournamentService(unitOfWork, mapper));
    }

    public IGameService GameService => _gameService.Value;
    public ITournamentService TournamentService => _tournamentService.Value;
}

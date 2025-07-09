using AutoMapper;
using Domain.Models.Entities;
using Tournament.DTOs.Games;
using Tournament.DTOs.Tournaments;

namespace Tournament.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings()
        {
            CreateMap<TournamentDetails, TournamentDto>();
            CreateMap<Game, GameDto>();
            CreateMap<TournamentForCreationDto, TournamentDetails>();
            CreateMap<GameForCreationInTournamentControllerDto, Game>();
            CreateMap<GameForCreationDto, Game>();
            CreateMap<GameForUpdateDto, Game>();
            CreateMap<GameDto, Game>();
            CreateMap<TournamentDetails, TournamentDto>().ReverseMap();

            //test
        }
    }
}

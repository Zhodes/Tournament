using AutoMapper;
using Domain.Core.Entities;
using Domain.Core.Dtos;

namespace Domain.Presentation.Data
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

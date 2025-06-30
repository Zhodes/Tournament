using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Dtos;

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


        }
    }
}

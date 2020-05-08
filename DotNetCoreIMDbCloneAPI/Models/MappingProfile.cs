using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreIMDbCloneAPI.Models.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDetailDto>()
               // .ForMember(x=> x.Movie, opt => opt.MapFrom(ps => ps))
                ;
            CreateMap<IEnumerable<Actor>, MovieDetailDto>().ForMember(x=> x.Actors, opt => opt.MapFrom(ps => ps));
            CreateMap<IEnumerable<Genre>, MovieDetailDto>().ForMember(x => x.Genres, opt => opt.MapFrom(ps => ps));

              CreateMap<MovieDetailDto, Movie>();
            //  CreateMap<MovieDetailDto, IEnumerable<MovieActor>>()
            //.ForMember(x=> x.Select(y=>y.Actor), opt => opt.MapFrom(ps => ps.Actors))
            //  .ForMember(x => x.Select(y=>y.Actor), opt => opt.MapFrom( ps => ps.Actors))
            //.ForMember(x=> x.MovieId, opt => opt.MapFrom(ps => ps.MovieId))
            //   ;


             CreateMap<Actor, MovieActor>();
            //  CreateMap<MovieDetailDto,MovieActor>().ForMember(x=> x.MovieId, opt => opt.));

            // CreateMap<MovieDetailDto, MovieActor>(); // it works


            CreateMap<Movie, MovieListDTO>();
          //  CreateMap<List<Actor>, MovieListDTO>().ForMember(x => x.Actors, opt => opt.MapFrom(ps => ps));
          //  CreateMap<List<Genre>, MovieListDTO>().ForMember(x => x.Genres, opt => opt.MapFrom(ps => ps));
        }
    }
}

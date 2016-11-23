using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyRecordCollection.Models;
using MyRecordCollection.ViewModels;
using MyRecordCollection.Dtos;

namespace MyRecordCollection
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Artist, ArtistFormViewModel>();
            CreateMap<ArtistFormViewModel, Artist>();

            CreateMap<Album, AlbumFormViewModel>();
            CreateMap<AlbumFormViewModel, Album>();

            CreateMap<Artist, ArtistDto>();
            CreateMap<ArtistDto, Artist>();
        }
    }
}
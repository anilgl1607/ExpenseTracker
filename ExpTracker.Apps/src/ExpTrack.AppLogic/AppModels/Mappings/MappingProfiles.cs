using ExpTrack.AppModels.DTOs;
using AutoMapper;
using ExpTrack.DbAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.AppModels.Mappings
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserReadDto>()
                .ReverseMap();

            CreateMap<User, UserCreateDto>()
                .ReverseMap();

            CreateMap<RefreshToken,RefreshTokenReadDto>()
                .ReverseMap();

            CreateMap<Expense,ExpenseReadDto>()
                .ReverseMap();

            CreateMap<Expense, ExpenseCreateDto>()
                .ReverseMap();

            CreateMap<Category, CategoryReadDto>()
                .ReverseMap();

            CreateMap<Category, CategoryCreateDto>()
                .ReverseMap();
        }
    }
}

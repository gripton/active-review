using ARR.Data.Entities;
using ARR.API.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARR.API.Models
{
    public class IndexMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Account, AccountIndex>();
            Mapper.CreateMap<AccountIndex, Account>();
            Mapper.CreateMap<ReviewSession, ReviewIndex>();
            Mapper.CreateMap<ReviewIndex, ReviewSession>();
        }
    }
}
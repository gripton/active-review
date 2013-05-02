using ARR.Data.Entities;
using ARR.Prototype.API.Models;
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
            Mapper.CreateMap<ReviewSession, ReviewIndex>();

        }
    }
}
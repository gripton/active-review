using ARR.Data.Entities;
using ARR.API.Models;
using ARR.Repository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARR.API.Models
{
    public static class ModelExtensions
    {
        public static ReviewSession ToNewSession(this ReviewIndex index)
        {
            return Mapper.Map<ReviewSession>(index);
        }

        public static ReviewSession ToSession(this ReviewIndex index, IReadContext<ReviewSession> readContext)
        {
            var session = readContext.Get(index.Id);
            return Mapper.Map(index, session);
        }
    }
}
using ARR.Data.Entities;
using AutoMapper;
using PracticalCode.WebSecurity.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class AccountMappingProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Account, WebSecurityUser>()
                .ForMember(w => w.Statistics, m => m.MapFrom(map => new WebSecurityUserStatistics
                                                                        {
                                                                            FailedPasswordAttempts = map.FailedPasswordAttempt,
                                                                            LastModified = map.LastModified,
                                                                            LastLogin = map.LastLogin,
                                                                            LastLoginAttempted = map.LastLoginAttempted,
                                                                            LastPasswordChanged = map.LastPasswordChanged
                                                                        }))
                .ForMember(w => w.UserInfo, m => m.MapFrom(map => new Dictionary<string, string> 
                                                                      {
                                                                          { "Id", map.Id.ToString()}
                                                                      }));

            Mapper.CreateMap<WebSecurityUser, Account>()
                .ForMember(a => a.Id, m => m.MapFrom(map => map.UserInfo["Id"]))
                .ForMember(a => a.FailedPasswordAttempt, m => m.MapFrom(map => map.Statistics.FailedPasswordAttempts))
                .ForMember(a => a.LastModified, m => m.MapFrom(map => map.Statistics.LastModified))
                .ForMember(a => a.LastLogin, m => m.MapFrom(map => map.Statistics.LastLogin))
                .ForMember(a => a.LastLoginAttempted, m => m.MapFrom(map => map.Statistics.LastLoginAttempted))
                .ForMember(a => a.LastPasswordChanged, m => m.MapFrom(map => map.Statistics.LastPasswordChanged));

        }
    }
}
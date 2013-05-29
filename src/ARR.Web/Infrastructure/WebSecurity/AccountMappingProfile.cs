using System.Collections.Generic;

using ARR.Data.Entities;

using AutoMapper;
using PracticalCode.WebSecurity.Infrastructure.Data;

namespace ARR.Web.Infrastructure.WebSecurity
{
    public class AccountMappingProfile : Profile
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
                                                                          { "Id", map.Id.ToString()},
                                                                          { "EmailAddress", map.EmailAddress ?? string.Empty},
                                                                          { "ScreenName", map.ScreenName ?? string.Empty},
                                                                          { "Organization", map.Organization ?? string.Empty},
                                                                          { "AreaOfExpertise", map.AreaOfExpertise ?? string.Empty},
                                                                      }));

            Mapper.CreateMap<WebSecurityUser, Account>()
                .ForMember(a => a.Id, m => m.MapFrom(map => map.UserInfo["Id"]))
                .ForMember(a => a.EmailAddress, m => m.MapFrom(map => map.UserInfo["EmailAddress"]))
                .ForMember(a => a.ScreenName, m => m.MapFrom(map => map.UserInfo["ScreenName"]))
                .ForMember(a => a.Organization, m => m.MapFrom(map => map.UserInfo["Organization"]))
                .ForMember(a => a.AreaOfExpertise, m => m.MapFrom(map => map.UserInfo["AreaOfExpertise"]))
                .ForMember(a => a.FailedPasswordAttempt, m => m.MapFrom(map => map.Statistics.FailedPasswordAttempts))
                .ForMember(a => a.LastModified, m => m.MapFrom(map => map.Statistics.LastModified))
                .ForMember(a => a.LastLogin, m => m.MapFrom(map => map.Statistics.LastLogin))
                .ForMember(a => a.LastLoginAttempted, m => m.MapFrom(map => map.Statistics.LastLoginAttempted))
                .ForMember(a => a.LastPasswordChanged, m => m.MapFrom(map => map.Statistics.LastPasswordChanged));

        }
    }
}
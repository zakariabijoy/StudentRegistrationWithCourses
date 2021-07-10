using AutoMapper;
using StudentRegistration.Api.DTOs;
using StudentRegistration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Api.Mapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Course, CourseCheckBoxDto>().ForMember(d => d.Id, opt => opt.MapFrom(s => s.CourseId))
                                                    .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Name))
                                                            .ForMember(d => d.Credit, opt => opt.MapFrom(s => s.Credit));

            CreateMap<CourseCheckBoxDto, Course>().ForMember(d => d.CourseId, opt => opt.MapFrom(s => s.Id))
                                                    .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Title))
                                                            .ForMember(d => d.Credit, opt => opt.MapFrom(s => s.Credit));


        }
    }
}

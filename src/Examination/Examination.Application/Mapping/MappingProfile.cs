using AutoMapper;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Exam, ExamDto>().ReverseMap();
        }
    }
}

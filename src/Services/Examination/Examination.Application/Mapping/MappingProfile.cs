using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;

using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Dtos.Categories;
using Examination.Dtos.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Category = Examination.Domain.AggregateModels.CategoryAggregate.Category;

namespace Examination.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Exam, ExamDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}

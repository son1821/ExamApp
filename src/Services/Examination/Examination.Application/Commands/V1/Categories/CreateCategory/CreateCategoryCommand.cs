﻿using Examination.Shared.Categories;
using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands.V1.Categories.CreateCategory
{
    public class CreateCategoryCommand : IRequest<ApiResult<CategoryDto>>
    {
        public string Name { set; get; } = string.Empty;
        public string UrlPath { get; set; } = string.Empty;
    }
}

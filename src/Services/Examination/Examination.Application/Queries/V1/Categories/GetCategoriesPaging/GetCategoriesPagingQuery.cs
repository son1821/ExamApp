﻿using Examination.Shared.Categories;
using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Queries.V1.Categories.GetCategoriesPaging
{
    public class GetCategoriesPagingQuery : IRequest<ApiResult<PagedList<CategoryDto>>>
    {
        public string SearchKeyword { get; set; } = string.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

    }
}

﻿using Examination.Dtos.Categories;
using Examination.Dtos.SeedWork;
using Examination.Shared.Questions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Queries.V1.Questions.GetQuestionsPaging
{
    public class GetQuestionsPagingQuery : IRequest<PagedList<QuestionDto>>
    {
        public string SearchKeyword { get; set; } = string.Empty;
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
}

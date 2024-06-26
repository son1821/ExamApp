﻿using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Queries.V1.Exams.GetExamById
{
    public class GetExamByIdQuery : IRequest<ApiResult<ExamDto>>
    {
        public GetExamByIdQuery(string id)
        {
            Id = id;
        }
        public string Id { set; get; }
    }
}

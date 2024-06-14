using Amazon.Runtime.Internal;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Queries.V1.ExamResults.GetExamResultsPaging
{
    public class GetExamResultsPagingQuery : IRequest<ApiResult<PagedList<ExamResultDto>>>
    {
        public string SearchKeyword { get; set; } = string.Empty;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}

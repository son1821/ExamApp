using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands.V1.ExamResults.StarExam
{
    public class StartExamCommand : IRequest<ApiResult<ExamResultDto>>
    {
      
        public string ExamId { get; set; } = string.Empty;

   
    }
}

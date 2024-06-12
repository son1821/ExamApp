using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands.V1.ExamResults.FinishExam
{
    public class FinishExamCommand : IRequest<ApiResult<ExamResultDto>>
    {
        public string ExamResultId {  get; set; } = string.Empty;
    }
}

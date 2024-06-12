using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands.V1.ExamResults.SkipExam
{
    public class SkipExamCommand : IRequest<ApiResult<bool>>
    {
        public string ExamResultId { get; set; } = string.Empty;
    }
}

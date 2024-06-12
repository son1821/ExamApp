using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Examination.Application.Commands.V1.ExamResults.SkipExam
{
    public class SkipExamCommandHandler : IRequestHandler<SkipExamCommand, ApiResult<bool>>
    {
        private readonly IExamResultRepository _examResultRepository;

        public SkipExamCommandHandler(IExamResultRepository examResultRepository)
        {
          _examResultRepository = examResultRepository;
        }

        public async Task<ApiResult<bool>> Handle(SkipExamCommand request, CancellationToken cancellationToken)
        {
           var examResult = await _examResultRepository.GetDetails(request.ExamResultId);
            if(examResult == null)
            {
                return new ApiErrorResult<bool>(400, "ExamResult not found");
            }
            await _examResultRepository.DeleteAsync(request.ExamResultId);
            return new ApiSuccessResult<bool>(200,true);
        }
    }
}

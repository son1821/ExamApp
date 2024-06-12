using Amazon.Runtime.Internal;
using AutoMapper;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands.V1.ExamResults.SubmitQuestion
{
    public class SubmitQuestionCommandHandler : IRequestHandler<SubmitQuestionCommand, ApiResult<ExamResultDto>>
    {
        private readonly IExamResultRepository _examResultRepository;
        private readonly IMapper _mapper;

        public SubmitQuestionCommandHandler(IExamResultRepository examResultRepository, IMapper mapper)
        {
            _examResultRepository = examResultRepository;
           _mapper = mapper;
        }

        public async Task<ApiResult<ExamResultDto>> Handle(SubmitQuestionCommand request, CancellationToken cancellationToken)
        {
            var examResult = await _examResultRepository.GetDetails(request.ExamResultId);
            var question = examResult.QuestionResults.FirstOrDefault(x => x.Id == request.QuestionId);
            if(question == null)
            {
                return new ApiErrorResult<ExamResultDto>(400, "Question not found");
            }
            foreach(var item in question.Answers)
            {
                item.UserChosen = request.AnswerIds?.Contains(item.Id);
            }
            await _examResultRepository.UpdateAsync(examResult);
            var dto = _mapper.Map<ExamResultDto>(examResult);
            return new ApiSuccessResult<ExamResultDto>(200, dto);
        }
    }
}

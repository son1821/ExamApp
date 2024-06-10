using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Examination.Application.Commands.V1.Questions.DeleteQuestion
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, ApiResult<bool>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILogger<DeleteQuestionCommandHandler> _logger;

        public DeleteQuestionCommandHandler(
                IQuestionRepository questionRepository,
                ILogger<DeleteQuestionCommandHandler> logger
            )
        {
            _questionRepository = questionRepository;
            _logger = logger;

        }

        public async Task<ApiResult<bool>> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await _questionRepository.GetQuestionsByIdAsync(request.Id);
            if (itemToDelete == null)
            {
                _logger.LogError($"Item is not found {request.Id}");
                return new ApiErrorResult<bool>(400,$"itemToDelete not found: {request.Id} ");
            }

            await _questionRepository.DeleteAsync(request.Id);
            return new ApiSuccessResult<bool>(200,true, "Delete successful");
            
          
        }
    }
}
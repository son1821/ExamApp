using Examination.Domain.AggregateModels.QuestionAggregate;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Examination.Application.Commands.V1.Questions.DeleteQuestion
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, bool>
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

        public async Task<bool> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var itemToUpdate = await _questionRepository.GetQuestionsByIdAsync(request.Id);
            if (itemToUpdate == null)
            {
                _logger.LogError($"Item is not found {request.Id}");
                return false;
            }

            try
            {
                await _questionRepository.DeleteAsync(request.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}

using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Shared.Enums;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Examination.Application.Commands.V1.Questions.UpdateQuestion
{
    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, ApiResult<bool>>
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly ILogger<UpdateQuestionCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;
        public UpdateQuestionCommandHandler(
                IQuestionRepository questionRepository,
                ILogger<UpdateQuestionCommandHandler> logger,
                IMapper mapper,
                ICategoryRepository categoryRepository
            )
        {
            _questionRepository = questionRepository;
            _logger = logger;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        public async Task<ApiResult<bool>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {

            if (request.Answers?.Count(x => x.IsCorrect) > 1 && request.QuestionType == QuestionType.SingleSelection)
            {

                return new ApiErrorResult<bool>(200, "Single choice question cannot have multiple correct answers.");
            }
            var category = await _categoryRepository.GetCategoriesByIdAsync(request.CategoryId);

            //Generate ObjectID for new anwers
            foreach (var item in request.Answers)
            {
                if (string.IsNullOrEmpty(item.Id))
                {
                    item.Id = ObjectId.GenerateNewId().ToString();
                }
            }

            var answers = _mapper.Map<List<AnswerDto>, List<Answer>>(request.Answers);

            var itemToUpdate = await _questionRepository.GetQuestionsByIdAsync(request.Id);
            if (itemToUpdate == null)
            {
                _logger.LogError($"Item is not found {request.Id}");
                return new ApiErrorResult<bool>(200, $"itemToUpdate is not found {request.Id}");
            }

            itemToUpdate.Content = request.Content;
            itemToUpdate.QuestionType = request.QuestionType;
            itemToUpdate.Level = request.Level;
            itemToUpdate.CategoryId = request.CategoryId;
            itemToUpdate.Answers = answers;
            itemToUpdate.Explain = request.Explain;
            itemToUpdate.CategoryName = category.Name;
      
            await _questionRepository.UpdateAsync(itemToUpdate);

            var result = _mapper.Map<Question, QuestionDto>(itemToUpdate);

            return new ApiSuccessResult<bool>(200, true,"Update successful");
        }
    }
}

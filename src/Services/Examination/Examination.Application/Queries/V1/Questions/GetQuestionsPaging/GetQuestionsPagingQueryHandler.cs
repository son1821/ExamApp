
using AutoMapper;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Dtos.SeedWork;
using Examination.Shared.Questions;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;


namespace Examination.Application.Queries.V1.Questions.GetQuestionsPaging
{
    public class GetQuestionsPagingQueryHandler : IRequestHandler<GetQuestionsPagingQuery, PagedList<QuestionDto>>
    {

        private readonly IQuestionRepository _questionRepository;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly IMapper _mapper;
        private readonly ILogger<GetQuestionsPagingQueryHandler> _logger;

        public GetQuestionsPagingQueryHandler(
                IQuestionRepository questionRepository,
                IMapper mapper,
                ILogger<GetQuestionsPagingQueryHandler> logger,
                IClientSessionHandle clientSessionHandle
            )
        {
            _questionRepository = questionRepository ?? throw new ArgumentNullException(nameof(questionRepository));
            _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<PagedList<QuestionDto>> Handle(GetQuestionsPagingQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("BEGIN: GetQuestionsPagingQueryHandler");

            var result = await _questionRepository.GetQuestionsPagingAsync(request.SearchKeyword, request.PageIndex, request.PageSize);
            var items = _mapper.Map<List<QuestionDto>>(result.Item1);

            _logger.LogInformation("END: GetQuestionsPagingQueryHandler");
            return new PagedList<QuestionDto>(items, result.Item2, request.PageIndex, request.PageSize);
        }
    }
}


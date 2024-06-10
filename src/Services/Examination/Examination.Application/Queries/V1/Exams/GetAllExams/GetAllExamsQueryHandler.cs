
using AutoMapper;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;


namespace Examination.Application.Queries.V1.Exams.GetAllExams
{
    public class GetAllExamsQueryHandler : IRequestHandler<GetAllExamsQuery, ApiResult<IEnumerable<ExamDto>>>
    {
        private readonly IExamRepository _examRepository;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllExamsQueryHandler> _logger;

        public GetAllExamsQueryHandler(IExamRepository examRepository,
            IMapper mapper,
            IClientSessionHandle clientSessionHandle, ILogger<GetAllExamsQueryHandler> logger)
        {
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<ApiResult<IEnumerable<ExamDto>>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("BEGIN:GetAllExamsQueryHandler");

            var exams = await _examRepository.GetAllExamAsync();
            var examDtos = _mapper.Map<IEnumerable<ExamDto>>(exams);

            _logger.LogInformation("END:GetAllExamsQueryHandler");
            return new ApiSuccessResult<IEnumerable<ExamDto>>(200,examDtos);
        }
    }
}

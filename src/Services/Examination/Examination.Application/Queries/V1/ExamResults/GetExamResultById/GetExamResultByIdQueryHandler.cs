﻿using AutoMapper;
using Examination.Application.Queries.V1.Categories.GetCategoryById;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Queries.V1.ExamResults.GetExamResultById
{
    public class GetExamResultByIdQueryHandler : IRequestHandler<GetExamResultByIdQuery, ApiResult<ExamResultDto>>
    {
        private readonly IExamResultRepository _examResultRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetExamResultByIdQueryHandler> _logger;
        public GetExamResultByIdQueryHandler(IExamResultRepository examResultRepository,
                                            IMapper mapper, ILogger<GetExamResultByIdQueryHandler> logger)
        {
            _examResultRepository = examResultRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResult<ExamResultDto>> Handle(GetExamResultByIdQuery request, CancellationToken cancellationToken)
        {

            _logger.LogInformation("BEGIN: GetExamResultByIdQueryHandler");

            var result = await _examResultRepository.GetDetails(request.Id);
            var item = _mapper.Map<ExamResultDto>(result);

            _logger.LogInformation("END: GetExamResultByIdQueryHandler");

            return new ApiSuccessResult<ExamResultDto>(200, item);

        }
    }
}

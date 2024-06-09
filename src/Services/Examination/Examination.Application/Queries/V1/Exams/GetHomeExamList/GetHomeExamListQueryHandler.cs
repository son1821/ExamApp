﻿using Amazon.Runtime.Internal.Util;
using AutoMapper;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Queries.V1.Exams.GetHomeExamList
{
    public class GetHomeExamListQueryHandler : IRequestHandler<GetHomeExamListQuery, ApiResult<IEnumerable<ExamDto>>>
    {
        private readonly IExamRepository _examRepository;
        private readonly IClientSessionHandle _clientSessionHandle;
        private readonly IMapper _mapper;
        private readonly ILogger<GetHomeExamListQueryHandler> _logger;

        public GetHomeExamListQueryHandler(IExamRepository examRepository,
            IMapper mapper,
            IClientSessionHandle clientSessionHandle, ILogger<GetHomeExamListQueryHandler> logger)
        {
            _examRepository = examRepository ?? throw new ArgumentNullException(nameof(examRepository));
            _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<ApiResult<IEnumerable<ExamDto>>> Handle(GetHomeExamListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("BEGIN:GetHomeExamListQueryHandler");

            var exams = await _examRepository.GetExamListAsync();
            var examDtos = _mapper.Map<IEnumerable<ExamDto>>(exams);

            _logger.LogInformation("END:GetHomeExamListQueryHandler");
            return new ApiSuccessResult<IEnumerable<ExamDto>>(examDtos);
        }
    }
}
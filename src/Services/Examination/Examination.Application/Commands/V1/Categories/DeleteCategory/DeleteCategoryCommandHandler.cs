using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands.V1.Categories.DeleteCategory
{
    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResult<bool>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<DeleteQuestionCommandHandler> _logger;

        public DeleteQuestionCommandHandler(
                ICategoryRepository categoryRepository,
                ILogger<DeleteQuestionCommandHandler> logger
            )
        {
            _categoryRepository = categoryRepository;
            _logger = logger;

        }

        public async Task<ApiResult<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var itemToDelete = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
            if (itemToDelete == null)
            {
                _logger.LogError($"Item is not found {request.Id}");
                return new ApiErrorResult<bool>(400, $"itemToDelete is not found {request.Id}");
            }

            
                await _categoryRepository.DeleteAsync(request.Id);
                return new ApiSuccessResult<bool>(200,true,"Delete Successful");
            
        }
    }
}
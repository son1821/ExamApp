using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Dtos.Categories;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands.V1.Categories.CreateCategory
{
    public class CreateQuestionCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateQuestionCommandHandler> _logger;

        public CreateQuestionCommandHandler(
                ICategoryRepository categoryRepository,
                ILogger<CreateQuestionCommandHandler> logger,
                 IMapper mapper
            )
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;

        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var itemToAdd = await _categoryRepository.GetCategoriesByNameAsync(request.Name);
            if (itemToAdd != null)
            {
                _logger.LogError($"Item name existed: {request.Name}");
                return null;
            }
            itemToAdd = new Category(ObjectId.GenerateNewId().ToString(), request.Name, request.UrlPath);
            try
            {
                await _categoryRepository.InsertAsync(itemToAdd);
                return _mapper.Map<Category, CategoryDto>(itemToAdd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}

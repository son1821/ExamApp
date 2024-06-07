using Examination.Shared.SeedWork;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands.V1.Categories.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest<ApiResult<bool>>
    {
        public string Id { get; set; } = string.Empty;
        public string Name { set; get; } = string.Empty;
        public string UrlPath { get; set; } = string.Empty;
    }
}

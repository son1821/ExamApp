﻿using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.WebUtilities;
using PortalApp.Services.Interfaces;

namespace PortalApp.Services
{
    public class ExamService : BaseService , IExamService
    {
        public ExamService(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, httpContextAccessor)
        {
        }

        public async Task<ApiResult<ExamDto>> GetExamByIdAsync(string id)
        {
            return await GetAsync<ExamDto>($"/api/v1/Exams/{id}", true);
        }

        public async Task<ApiResult<PagedList<ExamDto>>> GetExamsPagingAsync(ExamSearch searchInput)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageIndex"] = searchInput.PageNumber.ToString(),
                ["pageSize"] = searchInput.PageSize.ToString()
            };

            if (!string.IsNullOrEmpty(searchInput.Name))
                queryStringParam.Add("searchKeyword", searchInput.Name);

            if (!string.IsNullOrEmpty(searchInput.CategoryId))
                queryStringParam.Add("categoryId", searchInput.CategoryId);

            string url = QueryHelpers.AddQueryString("/api/v1/exams/paging", queryStringParam);

            var result = await GetAsync<PagedList<ExamDto>>(url, true);
            return result;
        }
    }
}

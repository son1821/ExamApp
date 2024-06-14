using AdminApp.Services.Interfaces;
using Examination.Shared.ExamResults;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using System.Net.Http.Json;

namespace AdminApp.Services
{
    public class ExamResultService : IExamResultService
    {
        public HttpClient _httpClient;

        public ExamResultService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResult<ExamResultDto>> GetExamResultByIdAsync(string id)
        {
            var result = await _httpClient.GetFromJsonAsync<ApiResult<ExamResultDto>>($"/api/examresults/{id}");
            return result;
        }

        public async Task<ApiResult<PagedList<ExamResultDto>>> GetExamResultsPagingAsync(ExamResultSearch ListSearch)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageIndex"] = ListSearch.PageNumber.ToString(),
                ["pageSize"] = ListSearch.PageSize.ToString()
            };

            if (!string.IsNullOrEmpty(ListSearch.keyWork))
                queryStringParam.Add("searchKeyword", ListSearch.keyWork);


            string url = QueryHelpers.AddQueryString("/api/v1/examresults/paging", queryStringParam);

            var result = await _httpClient.GetFromJsonAsync<ApiResult<PagedList<ExamResultDto>>>(url);
            return result;
        }
    }
}

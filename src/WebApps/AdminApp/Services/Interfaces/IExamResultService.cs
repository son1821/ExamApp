using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;

namespace AdminApp.Services
{
    public interface IExamResultService
    {
        Task<ApiResult<PagedList<ExamResultDto>>> GetExamResultsPagingAsync(ExamResultSearch ListSearch);
        Task<ApiResult<ExamResultDto>> GetExamResultByIdAsync(string id);
    }
}

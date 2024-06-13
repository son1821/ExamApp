using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortalApp.Services.Interfaces;

namespace PortalApp.Pages.Profile
{
    public class ExamHistoriesModel : PageModel
    {
        private readonly IExamResultService _examResultService;

        [BindProperty]
        public PagedList<ExamResultDto> ExamResults { set; get; } = new PagedList<ExamResultDto>();

        public ExamHistoriesModel(IExamResultService examResultService)
        {
            _examResultService = examResultService;
        }

        public async Task OnGetAsync([FromQuery] PagingParameters request)
        {
            var result = await _examResultService.GetExamResultsByUserIdPagingAsync(request);
            if (result.IsSuccessed)
            {
                ExamResults = result.ResultObj;
            }
        }
    }
}

using Examination.Domain.SeedWork;
using Examination.Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Domain.AggregateModels.ExamResultAggregate
{
    public interface IExamResultRepository : IRepositoryBase<ExamResult>
    {
        Task<PagedList<ExamResult>> GetExamResultsByUserIdPagingAsync(string uerId, int pageIndex, int pageSize);
        Task<PagedList<ExamResult>> GetExamResultsPagingAsync(string searchKeyword, int pageIndex, int pageSize);
        Task<ExamResult> GetDetails(string id);
    }
}

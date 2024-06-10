using Examination.Domain.SeedWork;
using Examination.Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Domain.AggregateModels.ExamAggregate
{
    public interface IExamRepository : IRepositoryBase<Exam>
    {
        Task<IEnumerable<Exam>> GetAllExamAsync();
        Task<Exam> GetExamByIdAsync(string id);
        Task<PagedList<Exam>> GetExamsPagingAsync(string categoryId,string searchKeyword, int pageIndex, int pageSize);
    }
}

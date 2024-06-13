using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Infrastructure.SeedWork;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Infrastructure.Repositories
{
    public class ExamResultRepository : BaseRepository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(
            IMongoClient mongoClient, 
            IOptions<ExamSettings> settings) : base(mongoClient, settings, Constants.Collections.ExamResult)
        {
        }

        public async Task<ExamResult> GetDetails(string id)
        {
            var filter = Builders<ExamResult>.Filter.Where(s => s.Id ==id);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<PagedList<ExamResult>> GetExamResultsByUserIdPagingAsync(string userId, int pageIndex, int pageSize)
        {
            FilterDefinition<ExamResult> filter = Builders<ExamResult>.Filter.Empty;
     

            if (!string.IsNullOrEmpty(userId))
                filter = Builders<ExamResult>.Filter.Eq(s => s.UserId, userId);

            var totalRow = await Collection.Find(filter).CountDocumentsAsync();
            var items = await Collection.Find(filter)
                .SortByDescending(x => x.ExamFinishDate)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PagedList<ExamResult>(items, totalRow, pageIndex, pageSize);
        }
    }
}

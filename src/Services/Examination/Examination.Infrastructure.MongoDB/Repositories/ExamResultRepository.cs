﻿using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office2010.Excel;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Infrastructure.SeedWork;
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
    }
}

﻿using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Domain.AggregateModels.ExamAggregate;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Domain.AggregateModels.QuestionAggregate;
using Examination.Domain.AggregateModels.UserAggregate;
using Examination.Infrastructure.Repositories;

namespace Examination.API
{
    public static class ServicesRegister
    {
        public static void RegisterCustomServices(this IServiceCollection services) 
        {
            services.AddTransient<IExamRepository, ExamRepository>();
            services.AddTransient<IExamResultRepository, ExamResultRepository>();
            services.AddTransient<IQuestionRepository, QuestionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
        }
    }
}

﻿using Examination.Shared.SeedWork;
using Examination.Shared.Questions;

namespace AdminApp.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<PagedList<QuestionDto>> GetQuestionsPagingAsync(QuestionSearch ListSearch);
        Task<QuestionDto> GetQuestionByIdAsync(string id);
        Task<bool> CreateAsync(CreateQuestionRequest request);
        Task<bool> UpdateAsync(UpdateQuestionRequest request);
        Task<bool> DeleteAsync(string id);
    }
}


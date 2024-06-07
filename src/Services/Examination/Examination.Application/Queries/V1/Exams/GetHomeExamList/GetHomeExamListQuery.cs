
using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;


namespace Examination.Application.Queries.V1.Exams.GetHomeExamList
{
    public class GetHomeExamListQuery : IRequest<ApiResult<IEnumerable<ExamDto>>>
    {
    }
}

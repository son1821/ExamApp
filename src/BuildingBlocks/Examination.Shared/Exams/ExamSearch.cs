using Examination.Shared.SeedWork;

namespace Examination.Shared.Exams
{
    public class ExamSearch : PagingParameters
    {
        public string CategoryId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}

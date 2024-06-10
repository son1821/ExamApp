using Examination.Shared.Enums;
using Examination.Shared.Questions;


namespace Examination.Shared.Exams
{
    public class ExamDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public string ShortDesc { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public int NumberOfQuestions { get; set; }

        public int? DurationInMinutes { get; set; }

        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();

        public Level Level { get; set; }

        public DateTime DateCreated { get; set; }

        public string OwnerUserId { get; set; } = string.Empty;

        public int NumberOfQuestionCorrectForPass { get; set; }

        public bool IsTimeRestricted { get; set; }
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }
}

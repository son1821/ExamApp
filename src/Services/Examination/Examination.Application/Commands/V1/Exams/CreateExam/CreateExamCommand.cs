using Examination.Shared.Enums;
using Examination.Shared.Exams;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Examination.Application.Commands.V1.Exams.CreateExam
{
    public class CreateExamCommand : IRequest<ApiResult<ExamDto>>
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string ShortDesc { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        [Required]
        public int NumberOfQuestions { get; set; }

        public string Duration { get; set; } = string.Empty;

        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();

        [Required]
        public Level Level { get; set; }

        [Required]
        public int NumberOfQuestionCorrectForPass { get; set; }

        [Required]
        public bool IsTimeRestricted { get; set; }

        public bool AutoGenerateQuestion { set; get; }

        [Required]
        public string CategoryId { get; set; } = string.Empty;
    }
}

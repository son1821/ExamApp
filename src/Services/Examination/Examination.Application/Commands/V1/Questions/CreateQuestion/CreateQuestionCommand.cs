using Examination.Dtos.Enums;
using Examination.Shared.Questions;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Examination.Application.Commands.V1.Questions.CreateQuestion
{
    public class CreateQuestionCommand : IRequest<QuestionDto>
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public QuestionType QuestionType { get; set; }

        [Required]
        public Level Level { set; get; }

        [Required]
        public string CategoryId { get; set; } = string.Empty;

        [Required]
        public List<AnswerDto> Answers { set; get; } = new List<AnswerDto>();

        public string Explain { get; set; } = string.Empty;
    }
}

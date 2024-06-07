using Examination.Shared.Enums;

using System.ComponentModel.DataAnnotations;


namespace Examination.Shared.Questions
{
    public class UpdateQuestionRequest
    {

        [Required]
        public string Id { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty ;

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

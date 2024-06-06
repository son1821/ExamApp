using Examination.Dtos.Enums;
using Examination.Shared.Questions;
using MediatR;
using System.ComponentModel.DataAnnotations;


namespace Examination.Application.Commands.V1.Questions.UpdateQuestion
{
    public class UpdateQuestionCommand : IRequest<bool>

    {

        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;

        
        public QuestionType QuestionType { get; set; }

       
        public Level Level { set; get; }

        
        public string CategoryId { get; set; } = string.Empty;

     
        public List<AnswerDto> Answers { set; get; } = new List<AnswerDto>();

        public string Explain { get; set; } = string.Empty;
    }
}

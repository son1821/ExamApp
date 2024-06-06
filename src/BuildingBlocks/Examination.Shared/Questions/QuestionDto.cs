using Examination.Dtos.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Shared.Questions
{
    public class QuestionDto
    {
        public string Content { get; set; } = string.Empty;

        public QuestionType QuestionType { get; set; }

        public Level Level { set; get; }

        public string CategoryId { get; set; } = string.Empty;

        public IEnumerable<AnswerDto> Answers { set; get; } = Enumerable.Empty<AnswerDto>();

        public string Explain { get; set; } = string.Empty;

        public DateTime DateCreated { get; set; }

        public string OwnerUserId { get; set; } = string.Empty;
    }
}

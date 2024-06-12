using Examination.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Shared.ExamResults
{
    public class QuestionResultDto
    {
        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty ;
        public QuestionType QuestionType { get; set; }
        public Level Level { get; set; }
        public string Explain { get; set; } = string.Empty;
        public List<AnswerResultDto> Answers { set; get; } = new List<AnswerResultDto>();
        public bool? Result { get; set; }
    }
}

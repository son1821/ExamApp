using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Shared.ExamResults
{
    public class NextQuestionRequest
    {
        public string ExamResultId { get; set; } = string.Empty;

        public string QuestionId { get; set; } = string.Empty;

        public List<string> AnswerIds { get; set; } = new List<string>();
    }
}

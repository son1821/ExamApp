using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Shared.ExamResults
{
    public class AnswerResultDto
    {
        public string Id { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; 
        public bool? UserChosen { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Shared.ExamResults
{
    public class ExamResultDto
    {
        public string Id { set; get; } = string.Empty;
        public string ExamId { get; set; } = string.Empty;

        public string ExamTitle {  set; get; } = string.Empty;
        public string UserId { set; get; } = string.Empty;

        public List<QuestionResultDto> QuestionResults { get; set; } = new List<QuestionResultDto>();

        public DateTime ExamStartDate { get; set; }

        public DateTime? ExamFinishDate { get; set; }

        public bool? Passed { get; set; }

        public bool Finished { get; set; }
    }
}

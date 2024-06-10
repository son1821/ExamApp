using Examination.Shared.Enums;
using Examination.Shared.Questions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Examination.Shared.Exams
{
    public class UpdateExamRequest
    {
        [Required]
 
        public string Id { get; set; } = String.Empty;

        [Required]
        public string Name { get; set; } = String.Empty;

        [Required]
        public string ShortDesc { get; set; } = String.Empty;

        public string Content { get; set; } = String.Empty;

        [Required]
        public int NumberOfQuestions { get; set; }

        public int? DurationInMinutes { get; set; }

        public List<QuestionDto> Questions { get; set; } = new List<QuestionDto>();

        [Required]
        public Level Level { get; set; }

        [Required]
        public int NumberOfQuestionCorrectForPass { get; set; }

        [Required]
        public bool IsTimeRestricted { get; set; }

        public bool AutoGenerateQuestion { set; get; }

        [Required]
        public string CategoryId { get; set; } = String.Empty;
    }
}

using Examination.Dtos.Enums;
using Examination.Shared.Questions;
using System.ComponentModel.DataAnnotations;


namespace Examination.Dtos.Categories
{
    public class CreateCategoryRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string UrlPath { get; set; } = string.Empty;
    }
}

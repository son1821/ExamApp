using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Dtos.Categories
{
    public class UpdateCategoryRequest
    {
        [Required]
        public string Id { set; get; } = string.Empty;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string UrlPath { get; set; } = string.Empty ;
    }
}

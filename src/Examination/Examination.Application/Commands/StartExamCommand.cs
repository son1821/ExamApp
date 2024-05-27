using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Application.Commands
{
    public class StartExamCommand : IRequest<bool>
    {
        public string UserId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string ExamId { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
    }
}

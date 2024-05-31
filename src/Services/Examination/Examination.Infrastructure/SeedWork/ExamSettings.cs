using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.Infrastructure.SeedWork
{
    public class ExamSettings
    {
        public string IdentityUrl { get; set; } = string.Empty;

        public DatabaseSettings DatabaseSettings { get; set; } = new DatabaseSettings();
    }

    public class DatabaseSettings
    {
        public string ConnectionString { set; get; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;

    }
}

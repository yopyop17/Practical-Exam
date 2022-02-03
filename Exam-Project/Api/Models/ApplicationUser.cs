using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Models
{
    public class ApplicationUser : IdentityUser<long>
    {
        public DateTime? RegisteredDate { get; set; }
    }
    public class ApplicationRole : IdentityRole<long> { }
}

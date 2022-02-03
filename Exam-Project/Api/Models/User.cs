using Exam_Project.Api.Entities;
using Exam_Project.Api.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            Sessions = new HashSet<Sessions>();
        }
        public long Id { get; set; }
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required, MaxLength(100)]
        public string Email { get; set; }
        public virtual ICollection<Sessions> Sessions { get; set; }
    }
}

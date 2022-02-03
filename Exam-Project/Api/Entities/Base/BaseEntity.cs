using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Entities.Base
{
    public abstract class BaseEntity
    {
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }

        public void MarkDeleted(DateTime? dateDeleted = null)
        {
            this.DateDeleted = dateDeleted ?? DateTime.UtcNow;
        }
    }
}

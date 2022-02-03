using Exam_Project.Api.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Models
{
    public class Order : BaseEntity
    {
        public long Id { get; set; }
        public double Stock { get; set; }
        public double Quantity { get; set; }
        public string ProductName { get; set; }
        public User Customer { get; set; }
        public long? CustomerId { get; set; }
    }
}

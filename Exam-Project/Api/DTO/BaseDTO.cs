using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam_Project.Api.DTO
{
    public class BaseDTO
    {
        [JsonProperty("dt_c")]
        public DateTime DateCreated { get; set; }
        [JsonProperty("dt_m")]
        public DateTime? DateModified { get; set; }
        [JsonProperty("dt_d")]
        public DateTime? DateDeleted { get; set; }
    }
}

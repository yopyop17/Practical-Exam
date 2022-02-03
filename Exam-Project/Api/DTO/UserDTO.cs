using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.DTO
{
    public class UserDTO : BaseDTO
    {
        [JsonProperty("pass")]
        public string Password { get; set; }
        [JsonProperty("i")]
        public long Id { get; set; }
        [JsonProperty("e")]
        public string Email { get; set; }
        [JsonProperty("f_n")]
        public string FirstName { get; set; }
        [JsonProperty("l_n")]
        public string LastName { get; set; }
        public string Message { get; set; }
    }
}

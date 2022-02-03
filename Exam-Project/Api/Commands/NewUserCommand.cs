using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Commands
{
    public class NewUserCommand
    {
        [JsonProperty("f_n")]
        public string FirstName { get; set; }
        [JsonProperty("l_n")]
        public string LastName { get; set; }
        [JsonProperty("e")]
        public string Email { get; set; }
        [JsonProperty("pass")]
        public string Password { get; set; }
    }
}

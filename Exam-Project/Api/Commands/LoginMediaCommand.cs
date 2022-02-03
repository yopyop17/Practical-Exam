using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Commands
{
    public class LoginCommand
    {
        [JsonProperty("user")]
        public string Username { get; set; }
        [JsonProperty("pass")]
        public string Password { get; set; }
    }
    public class LoginMediaCommand : LoginCommand
    {
        [JsonProperty("m_tok")]
        public string MediaToken { get; set; }

        [JsonProperty("m_id")]
        public string MediaID { get; set; }

        [JsonProperty("m_em")]
        public string MediaEmail { get; set; }

        [JsonProperty("m_dp")]
        public string MediaPicture { get; set; }

        [JsonProperty("m_fname")]
        public string MediaFirstName { get; set; }

        [JsonProperty("m_lname")]
        public string MediaLastName { get; set; }
    }
}

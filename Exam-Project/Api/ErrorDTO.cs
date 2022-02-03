using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api
{
    public class ErrorDTO
    {
        /// <summary>
        /// HTTP response code (4xx/5xx)
        /// </summary>
        [JsonProperty("s")]
        public int Status { get; set; }

        [JsonProperty("m")]
        public string Message { get; set; }

        [JsonProperty("ekey")]
        public string Key { get; set; }

        [JsonProperty("d")]
        public object Data { get; set; }

        [JsonProperty("p")]
        public string[] Parameter { get; set; }
    }
}

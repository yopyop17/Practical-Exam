using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam_Project.Api.Commands
{
    public class OrderCommand
    {
        [JsonProperty("i")]
        public long Id { get; set; }
        [JsonProperty("st")]
        public double Stock { get; set; }
        [JsonProperty("q")]
        public double Quantity { get; set; }
        [JsonProperty("pname")]
        public string ProductName { get; set; }
        public UserCommand Customer { get; set; }
    }
}

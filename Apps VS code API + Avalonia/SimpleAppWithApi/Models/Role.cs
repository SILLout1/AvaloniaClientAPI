using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleAppWithApi.Models
{
    public class Role
    {
        [JsonPropertyName("positionCode")]
        public int Id { get; set; }
        
        [JsonPropertyName("positionName")]
        public string Name { get; set; } = null!;
    }
}
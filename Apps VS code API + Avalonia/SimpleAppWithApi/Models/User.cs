using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleAppWithApi.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        
        [JsonPropertyName("surname")]
        public string Surname { get; set; } = null!;
        
        [JsonPropertyName("patronymic")]
        public string? Patronymic { get; set; }
        
        [JsonPropertyName("birthday")]
        public DateTime Birthday { get; set; }
        
        [JsonPropertyName("roleid")]
        public int Roleid { get; set; }
        
        [JsonPropertyName("roleName")]
        public string RoleName { get; set; } = null!;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleAppWithApi.DTO
{
    public class EmployeeCreateDto
{
     [JsonPropertyName("LastName")]
    public string LastName { get; set; } = "";

    [JsonPropertyName("FirstName")]
    public string FirstName { get; set; } = "";

    [JsonPropertyName("MiddleName")]
    public string? MiddleName { get; set; }

    [JsonPropertyName("BirthDate")]
    public DateOnly BirthDate { get; set; }

    [JsonPropertyName("PositionCode")]
    public int PositionCode { get; set; }
}
}
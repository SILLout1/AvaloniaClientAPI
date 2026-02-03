using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SimpleAppWithApi.DTO
{
    public class EmployeeEditDto
    {
    public string? Surname { get; set; }
    public string? Name { get; set; }
    public string? Patronymic { get; set; }
    public DateTime? Birthday { get; set; }
    public int? Roleid { get; set; }
    }
}
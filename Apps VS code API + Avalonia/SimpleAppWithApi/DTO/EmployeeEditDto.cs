using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAppWithApi.DTO
{
    public class EmployeeEditDto
    {
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string? MiddleName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? PositionCode { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleAppWithApi.DTO
{
    public class CreatedEmployeeResponse
    {
        public int EmployeeId { get; set; }
        public string? PhotoPath { get; set; }
    }
}
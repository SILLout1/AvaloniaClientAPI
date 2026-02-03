using System;
using System.Collections.Generic;

namespace MYApi2.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public DateOnly? HireDate { get; set; }

    public decimal? Salary { get; set; }

    public string? Phone { get; set; }

    public int? PositionCode { get; set; }
    
     public string? PhotoPath { get; set; }

    public virtual Position? PositionCodeNavigation { get; set; }
}

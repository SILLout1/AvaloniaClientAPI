using System;
using System.Collections.Generic;

namespace MYApi2.Models;

public partial class Position
{
    
    public int PositionCode { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

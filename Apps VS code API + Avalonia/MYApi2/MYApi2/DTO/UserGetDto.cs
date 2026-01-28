using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MYApi2.DTO
{
    public class UserGetDto
    {
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string? Patronymic { get; set; }
    public DateOnly Birthday { get; set; }
    public int Roleid { get; set; }
    public string RoleName { get; set; } = null!;
    }
}
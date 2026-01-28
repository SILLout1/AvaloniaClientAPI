using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MYApi2.DTO;
using MYApi2.Models;

namespace MYApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class EmployeesController : ControllerBase
    {
        public readonly Zadanie2Context _context;

        public EmployeesController(Zadanie2Context context)
        {
            _context = context;
        }

[HttpGet]
public async Task<ActionResult<IEnumerable<UserGetDto>>> GetEmployees()
{
    var employees = await _context.Employees
        .Include(e => e.PositionCodeNavigation)
        .Select(e => new UserGetDto
        {
            Id = e.EmployeeId,
            Name = e.FirstName,
            Surname = e.LastName,
            Patronymic = e.MiddleName,
            Birthday = e.BirthDate ?? default,
            Roleid = e.PositionCode ?? 0,
            RoleName = e.PositionCodeNavigation!.PositionName
        }).ToListAsync();
    
    return employees;
}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeesId(int id)
        {
            var employes = await _context.Employees
                .Where(e=>e.EmployeeId == id)
                .Select(e => new EmployeesDto
                {
                    employeeId = e.EmployeeId,
                    lastName = e.LastName,
                    firstName = e.FirstName,
                    middleName = e.MiddleName,
                    birthDate = e.BirthDate,
                    hireDate = e.HireDate,
                    salary = e.Salary,
                    phone = e.Phone,
                }).FirstOrDefaultAsync();
            if (employes == null) return NotFound("Сотрудник не найден");
            return Ok(employes);
        }

        [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto dto)
    {
        var lastId = await _context.Employees.MaxAsync(e => (int?)e.EmployeeId) ?? 0;

        var employee = new Employee
        {
            EmployeeId = lastId + 1,
            LastName = dto.LastName,
            FirstName = dto.FirstName,
            MiddleName = dto.MiddleName,
            BirthDate = DateOnly.FromDateTime(dto.BirthDate),
            PositionCode = dto.PositionCode
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return Ok(employee.EmployeeId);
    }
        [HttpPut("{id}")]
public async Task<IActionResult> UpdateEmployee(int id, EmployeeEditDto dto)
{
    var ev = await _context.Employees.FindAsync(id);
    if (ev == null) return NotFound();

    if (dto.LastName != null) ev.LastName = dto.LastName;
    if (dto.FirstName != null) ev.FirstName = dto.FirstName;
    if (dto.MiddleName != null) ev.MiddleName = dto.MiddleName;

    if (dto.BirthDate.HasValue)
        ev.BirthDate = DateOnly.FromDateTime(dto.BirthDate.Value);

    if (dto.PositionCode.HasValue)
        ev.PositionCode = dto.PositionCode.Value;

    await _context.SaveChangesAsync();
    return Ok();
}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var ev = await _context.Employees.FindAsync(id);

            if (ev == null)
                return NotFound();

            _context.Employees.Remove(ev);
            await _context.SaveChangesAsync();

            return Ok("Сотрудник удален");
        }
    }
}

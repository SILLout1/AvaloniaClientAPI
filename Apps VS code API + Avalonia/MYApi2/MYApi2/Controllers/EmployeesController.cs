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
            RoleName = e.PositionCodeNavigation!.PositionName,
            PhotoPath  = e.PhotoPath != null 
                       ? $"{Request.Scheme}://{Request.Host}/Uploads/{e.PhotoPath}" 
                       : null
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
        [RequestSizeLimit(10_000_000)] 
public async Task<IActionResult> CreateEmployee([FromForm] EmployeeCreateDto dto)
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

    if(dto.Photo != null && dto.Photo.Length > 0)
    {
        
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        if(!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

    // путь к папке Uploads в корне приложения
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.Photo.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

    // сохраняем файл на диск
        using(var stream = new FileStream(filePath, FileMode.Create))       
            await dto.Photo.CopyToAsync(stream);
        
        employee.PhotoPath = fileName;
    }

    _context.Employees.Add(employee);
    await _context.SaveChangesAsync();

    return Ok(new { employee.EmployeeId, employee.PhotoPath });
}

    [HttpPut("{id}")]
public async Task<IActionResult> UpdateEmployee(
    int id,
    [FromBody] EmployeeEditDto dto)
{
    var ev = await _context.Employees.FindAsync(id);
    if (ev == null) return NotFound();

    if (dto.Surname != null) ev.LastName = dto.Surname;
    if (dto.Name != null) ev.FirstName = dto.Name;
    if (dto.Patronymic != null) ev.MiddleName = dto.Patronymic;
    if (dto.Birthday.HasValue)
        ev.BirthDate = DateOnly.FromDateTime(dto.Birthday.Value);
    if (dto.Roleid.HasValue) ev.PositionCode = dto.Roleid.Value;

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

// http://localhost:5093/Uploads/ff3f0e36-8495-4692-91d0-09a747295107.png
    [HttpPost("{id}/uploadPhoto")]
    public async Task<IActionResult> UploadPhoto(int id, IFormFile file)
    {
        var employee = await _context.Employees.FindAsync(id);
        if (employee == null) return NotFound();

        if (file.Length > 0)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine("Uploads", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            employee.PhotoPath = fileName;
            await _context.SaveChangesAsync();

            return Ok(fileName); 
        }

        return BadRequest();
    }
    }
}

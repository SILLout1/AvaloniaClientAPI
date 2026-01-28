using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYApi2.DTO;
using MYApi2.Models;

namespace MYApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PosishonsController : ControllerBase
    {
        public readonly Zadanie2Context _context;
        public PosishonsController(Zadanie2Context context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> GetPosishions()
        {
            var role = await _context.Positions
                .Select(e => new RoleGEt
                {
                    positionCode = e.PositionCode,
                    positionName = e.PositionName,
                }
                ).ToListAsync();
            return Ok(role);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPosishions(int id)
        {
            var role = await _context.Positions
                .Where(e => e.PositionCode == id)
                .Select(e => new RoleGEt
                {
                    positionCode = e.PositionCode,
                    positionName = e.PositionName,
                }
                ).FirstOrDefaultAsync();
            return Ok(role);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePosishions([FromBody] RoleGEt dto)
        {
            var lastId = await _context.Positions.MaxAsync(e => (int?)e.PositionCode) ?? 0;
            var roles = new Position
            {
                PositionCode = lastId + 1,
                PositionName = dto.positionName,
            };
            await _context.Positions.AddAsync(roles);
            await _context.SaveChangesAsync();
            return Ok(roles.PositionCode);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePosishions(int id, [FromBody] RolePut dto)
        {
            var ev = await _context.Positions.FindAsync(id);
            if (ev == null)
                return NotFound("Роль не найдена");
            ev.PositionName = dto.PositionName;
            await _context.SaveChangesAsync();
            return Ok("Роль обновлена");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePosishions(int id)
        {
            var ev = await _context.Positions.FindAsync(id);
            if (ev == null)
                return NotFound();
            _context.Positions.Remove(ev);
            await _context.SaveChangesAsync();
            return Ok("Роль удалена");
        }
    }
}

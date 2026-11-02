using KonyvtarApi.DTOs;
using KonyvtarApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KonyvtarApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TelepulesController : ControllerBase
    {
        private readonly KonyvtarakContext _context;

        public TelepulesController(KonyvtarakContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(_context.Telepuleseks.Include(t => t.Konyvtaraks).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("TelepKonyvtar")]
        public IActionResult TelepKonyvtar()
        {
            try
            {
                return Ok(_context.Telepuleseks.Include(t => t.Konyvtaraks).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        //adott településhez jelenítse meg a település nevét, megyét és az ott lévő könyvtárak nevét
        [HttpGet("TelepulesNevKonyvtarai/{telepNev}")]
        public IActionResult TelepulesKonyvtarai(string telepNev)
        {
            try
            { 
                List<TelepulesKonyvtaraiDTO> result = _context.Telepuleseks.Include(t=>t.Konyvtaraks).Include(t=>t.Megye).
                    Where(t=>t.TelepNev == telepNev).
                    Select(k => new TelepulesKonyvtaraiDTO 
                    {
                        TelepNev = telepNev,
                        MegyeNev = k.Megye.MegyeNev
                    }).ToList();
               
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetTelepulesNevReszlet")]
        public IActionResult GetTelepulesNevReszlet(string nevReszlet)
        {
            try
            {
                var talalat = _context.Telepuleseks.Where(t => t.TelepNev.Contains(nevReszlet)).ToList();

                if (!talalat.Any())
                {
                    return NotFound("Nem található település");
                }

                return Ok(talalat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var talalat = _context.Telepuleseks.Find(id);
                if (talalat == null)
                {
                    return NotFound("Nem található település ezzel az azonosítóval: " + id);
                }
                return Ok(talalat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("PutTelepulesek")]
        public IActionResult PutTelepulesek([FromBody] Telepulesek ujTelepules)
        {
            try
            {
                _context.Telepuleseks.Update(ujTelepules);
                _context.SaveChanges();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteTelepulesek/{id}")]
        public IActionResult DeleteTelepulesek(int id)
        {
            try
            {
                var toDelete = _context.Telepuleseks.FirstOrDefault(t => t.Id == id);
                if (toDelete == null)
                {
                    return NotFound("Hiányzó település");
                }

                _context.Telepuleseks.Remove(toDelete);
                _context.SaveChanges();
                return Ok("Sikeres törlés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Post")]
        public IActionResult PostTelepulesek([FromBody] Telepulesek ujTelepules)
        {
            try
            {
                _context.Telepuleseks.Add(ujTelepules);
                _context.SaveChanges();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await _context.Telepuleseks.ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            try
            {
                var talalat = await _context.Telepuleseks.FindAsync(id);
                if (talalat == null)
                {
                    return NotFound("Nem található település ezzel az azonosítóval: " + id);
                }
                return Ok(talalat);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("PostAsync")]
        public async Task<IActionResult> PostTelepulesekAsync([FromBody] Telepulesek ujTelepules)
        {
            try
            {
                await _context.Telepuleseks.AddAsync(ujTelepules);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("PutAsync/{id}")]
        public async Task<IActionResult> PutTelepulesekAsync([FromBody] Telepulesek ujTelepules, int id)
        {
            try
            {
                var toUpdate = await _context.Telepuleseks.FindAsync(id);
                if (toUpdate == null)
                {
                    return NotFound("Hiányzó település");
                }

                _context.Entry(toUpdate).CurrentValues.SetValues(ujTelepules);
                await _context.SaveChangesAsync();
                return Ok("Sikeres mentés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("DeleteAsync/{id}")]
        public async Task<IActionResult> DeleteTelepulesekAsync(int id)
        {
            try
            {
                var toDelete = await _context.Telepuleseks.FindAsync(id);
                if (toDelete == null)
                {
                    return NotFound("Hiányzó település");
                }

                _context.Telepuleseks.Remove(toDelete);
                await _context.SaveChangesAsync();
                return Ok("Sikeres törlés");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
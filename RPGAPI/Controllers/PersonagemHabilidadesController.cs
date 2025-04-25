using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPGAPI.Data;
using RPGAPI.Models;

namespace RPGAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagemHabilidadesController : ControllerBase
    {
        private readonly DataContext _context;
        
        public PersonagemHabilidadesController(DataContext context)
        {
            _context = context;
        }
        
        [HttpGet("{personagemId}")]
        public async Task<ActionResult<List<PersonagemHabilidade>>> GetHabilidadePorPersonagem(int personagemId)
        {
            try
            {
                var personagemHabilidades = await _context.TB_PERSONAGENS_HABILIDADES
                    .Where(ph => ph.PersonagemId == personagemId).ToListAsync();

                return Ok(personagemHabilidades);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetHabilidades")]	
        public async Task<IActionResult> GetHabilidades()
        {
            try
            {
                List<Habilidade> habilidades = await _context.TB_HABILIDADES.ToListAsync();
                return Ok(habilidades);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonagemHabilidadesAsync(PersonagemHabilidade novoPersonagemHabilidade)
        {
            try
            {
                Personagem personagem = await _context.TB_PERSONAGENS
                    .Include(p => p.Arma)
                    .Include(p => p.PersonagemHabilidades).ThenInclude(ps => ps.Habilidade)
                    .FirstOrDefaultAsync(p => p.Id == novoPersonagemHabilidade.PersonagemId);
                
                if (personagem == null)
                    throw new System.Exception("Personagem não encontrado para o Id informado");
                
                Habilidade habilidade = await _context.TB_HABILIDADES
                                    .FirstOrDefaultAsync(h => h.Id == novoPersonagemHabilidade.HabilidadeId);
                
                if (habilidade == null)
                    throw new System.Exception("Habilidade não encontrada.");
                
                PersonagemHabilidade ph = new PersonagemHabilidade();
                ph.Personagem = personagem;
                ph.Habilidade = habilidade;
                await _context.TB_PERSONAGENS_HABILIDADES.AddAsync(ph);
                int linhaAfetadas = await _context.SaveChangesAsync();

                return Ok(linhaAfetadas);

            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeletarPersonagemHabilidade")]
        public async Task<IActionResult> DeletarPersonagemHabilidade(PersonagemHabilidade personagemHabilidade)
        {
            try
            {
                if (personagemHabilidade == null)
                    return NotFound("PersonagemHabilidade não encontrado.");

                PersonagemHabilidade personagemHabilidadeDb = await _context.TB_PERSONAGENS_HABILIDADES
                    .FirstOrDefaultAsync(ph => ph.PersonagemId == personagemHabilidade.PersonagemId && 
                                               ph.HabilidadeId == personagemHabilidade.HabilidadeId);


                if (personagemHabilidadeDb == null)
                    return NotFound("PersonagemHabilidade não encontrado.");

                _context.TB_PERSONAGENS_HABILIDADES.Remove(personagemHabilidadeDb);
                await _context.SaveChangesAsync();

                return Ok("PersonagemHabilidade deletado com sucesso.");
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
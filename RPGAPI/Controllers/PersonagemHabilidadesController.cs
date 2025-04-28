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

        [HttpGet("GetHabilidadePersonagem/{personagemId}")]
        public async Task<IActionResult> GetHabilidadePersonagem(int personagemId)
        {
            try
            {
                List<PersonagemHabilidade> lista = await _context.TB_PERSONAGENS_HABILIDADES
                    .Include(ph => ph.Habilidade)
                    .Where(ph => ph.PersonagemId == personagemId)
                    .ToListAsync();

                return Ok(lista);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetHabilidades")] // Bem parecido com o GetAll
        public async Task<IActionResult> GetHabilidades()
        {
            try
            {
                List<Habilidade> listaHabilidades = await _context.TB_HABILIDADES.ToListAsync();

                return Ok(listaHabilidades);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeletePersonagemHabilidade")]
        public async Task<IActionResult> DeletePersonagemHabilidade(PersonagemHabilidade informacoes)
        {
            try
            {
                var personagemHabilidade = await _context.TB_PERSONAGENS_HABILIDADES
                    .FirstOrDefaultAsync(ph => ph.PersonagemId == informacoes.PersonagemId && ph.HabilidadeId == informacoes.HabilidadeId);

                _context.TB_PERSONAGENS_HABILIDADES.Remove(personagemHabilidade);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
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
    }
}
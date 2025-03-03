using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPGAPI.Controllers;
using RPGAPI.Models;
using RPGAPI.Models.Enums;

namespace RPGAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagensExercicioController : ControllerBase
    {
        PersonagensExemploController _personagensExemploController = new PersonagensExemploController();
        private List<Personagem> personagens;

        public PersonagensExercicioController()
        {
            personagens = _personagensExemploController.GetPersonagens();
        }

        [HttpGet("GetByNome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            List<Personagem> listaBusca = personagens.FindAll(p => p.Nome.Contains(nome));
            if (listaBusca.Count == 0)
                return NotFound(new {
                    Mensagem = $"Nenhum Personagem Encontrado com esse nome {nome}",
                    StatusCode = 404
                });
            return Ok(listaBusca);
        }

        [HttpGet("GetByClerigoMago")]
        public IActionResult GetByClerigoMago()
        {
            List<Personagem> listaBusca = personagens.FindAll(p => p.Classe != ClasseEnum.Cavaleiro).OrderByDescending(p => p.PontosVida).ToList();
            return Ok(listaBusca);
        }

        [HttpGet("GetEstatisticas")]
        public IActionResult GetEstatisticas() {
            int somaInteligencia = 0;
            foreach (Personagem p in personagens) {
                somaInteligencia += p.Inteligencia;
            }

            return Ok(new {
                Mensagem = $"Quantidade de personagens: {personagens.Count()}, Soma das Inteligências: {somaInteligencia}",
                StatusCode = 200
            });
        }

        [HttpPost("PostValidacao")]
        public IActionResult PostValidacao(Personagem novoPersonagem) {
            if (novoPersonagem.Inteligencia < 10 || novoPersonagem.Inteligencia > 30)
                return BadRequest(new {
                    Mensagem = $"Defesa não pode ter valor menor que 10 ou inteligência maior que 30",
                    StatusCode = 400
                });
            personagens.Add(novoPersonagem);
            return Ok(personagens);
        }

        [HttpPost("PostValidacaoMago")]
        public IActionResult PostValidacaoMago(Personagem novoPersonagem) {
            if(novoPersonagem.Classe == ClasseEnum.Mago && novoPersonagem.Inteligencia < 35)
                return BadRequest(new {
                    Mensagem = $"Personagem Mago deve ter 35 ou mais de inteligência",
                    StatusCode = 400
                });
            personagens.Add(novoPersonagem);
            return Ok(personagens);
        }

        [HttpGet("GetByClasse/{enumId}")]
        public IActionResult GetByClasse(int enumId) {
            ClasseEnum enumDigitado = (ClasseEnum)enumId;
            
            List<Personagem> listaBusca = personagens.FindAll(p => p.Classe == enumDigitado);

            return Ok(listaBusca);
        }
    }
}
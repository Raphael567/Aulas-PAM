using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPGAPI.Models.DTOs
{
    public class AlterarSenhaDTO
    {
        public int UsuarioId { get; set; }
        public string PasswordString { get; set; }
        public string NovaSenha { get; set; }
    }
}
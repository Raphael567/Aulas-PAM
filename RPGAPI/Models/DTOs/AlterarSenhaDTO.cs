using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPGAPI.Models.DTOs
{
    public class AlterarSenhaDTO
    {
        public int IdUsuario { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
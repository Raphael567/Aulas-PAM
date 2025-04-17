using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RPGAPI.Data;
using RPGAPI.Models;
using RPGAPI.Models.DTOs;
using RPGAPI.Utils;

namespace RPGAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;

        public UsuariosController(DataContext context)
        {
            _context = context;
        }

        private async Task<bool> UsuarioExistente(string username)
        {
            if (await _context.TB_USUARIOS.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        [HttpGet ("GetAll")]
        public async Task<IActionResult> GettAllUsuarios()
        {
            try
            {
                List<Usuario> users = await _context.TB_USUARIOS.ToListAsync();
                return Ok(users);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost ("Registrar")]
        public async Task<IActionResult> RegistrarUsuario(Usuario user)
        {
            try
            {
                if (await UsuarioExistente(user.Username))
                    throw new System.Exception("Nome de usuário já existe");
                
                Criptografia.CriarPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
                user.PasswordString = string.Empty;
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                await _context.TB_USUARIOS.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(user.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
        {
            try
            {
                Usuario? usuario = await _context.TB_USUARIOS
                    .FirstOrDefaultAsync(x => x.Username.ToLower().Equals(credenciais.Username.ToLower()));
                
                if (usuario == null)
                    throw new System.Exception("Usuario não encontrado.");

                else if (!Criptografia.VerificarPasswordHash(credenciais.PasswordString, usuario.PasswordHash, usuario.PasswordSalt))
                    throw new System.Exception("Senha incorreta");
                else
                {
                    usuario.DataAcesso = DateTime.Now;
                    
                    _context.TB_USUARIOS.Update(usuario);
                    await _context.SaveChangesAsync();

                    return Ok(usuario);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        [HttpPut("AlterarSenha")]
        public async Task<IActionResult> AlterarSenhaUsuario(AlterarSenhaDTO request)
        {
            try
            {
                Usuario usuarioBanco = await _context.TB_USUARIOS.FirstOrDefaultAsync(u => u.Id == request.UsuarioId);
                if (usuarioBanco == null)
                    throw new System.Exception("Usuário não encontrado");
                
                else if (!Criptografia.VerificarPasswordHash(request.PasswordString, usuarioBanco.PasswordHash, usuarioBanco.PasswordSalt))
                    throw new System.Exception("Senha incorreta");

                Criptografia.CriarPasswordHash(request.NovaSenha, out byte[] novoHash, out byte[] novoSalt);
                usuarioBanco.PasswordHash = novoHash;
                usuarioBanco.PasswordSalt = novoSalt;
                
                _context.TB_USUARIOS.Update(usuarioBanco);
                await _context.SaveChangesAsync();

                return Ok("Senha alterado com sucesso!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // [HttpPut("AlterarSenha")]
        // public async Task<IActionResult> AlterarSenhaUsuario(Usuario credenciais, string novaSenha)
        // {
        //     try
        //     {
        //         Usuario usuarioBanco = await _context.TB_USUARIOS.FirstOrDefaultAsync(u => u.Id == credenciais.Id);
        //         if (usuarioBanco == null)
        //             throw new System.Exception("Usuário não encontrado");
                
        //         else if (!Criptografia.VerificarPasswordHash(credenciais.PasswordString, usuarioBanco.PasswordHash, usuarioBanco.PasswordSalt))
        //             throw new System.Exception("Senha incorreta");

        //         Criptografia.CriarPasswordHash(novaSenha, out byte[] novoHash, out byte[] novoSalt);
        //         usuarioBanco.PasswordHash = novoHash;
        //         usuarioBanco.PasswordSalt = novoSalt;
                
        //         _context.TB_USUARIOS.Update(usuarioBanco);
        //         await _context.SaveChangesAsync();

        //         return Ok("Senha alterado com sucesso!");
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }
    }
}
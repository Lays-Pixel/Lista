using Microsoft.AspNetCore.Mvc;
using Lista.Data;
using Lista.Models;
namespace Lista.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ListaContext _context;

        public UsuarioController(ListaContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        
        public IActionResult LoginUsuario(Usuario usuario)
        {
            var usuariodoBanco = _context.Usuario.Where(u => u.Email.Equals(usuario.Email) && u.Senha.Equals(usuario.Senha)).ToList();
            if (usuariodoBanco.Count == 0)
                return Unauthorized("Email ou Senha inválidos");

            HttpContext.Session.SetString("UsuarioLogado", usuariodoBanco[0].Id.ToString() );

            Response.Cookies.Append("UsuarioLogado", usuariodoBanco[0].Id.ToString(),
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30),
                    Secure = true,
                    HttpOnly = true,
                    SameSite = SameSiteMode.None
                });


            return Ok("Usuário Logado com sucesso!");

        }

        [HttpPost("Logout")]
        public IActionResult LogoutUsuario() 
        { 
            HttpContext.Session.Clear();
            Response.Cookies.Delete("UsuarioLogado");
            return Ok("Logout Completo");
        
        }


        [HttpPost("cadastrar")]
        public IActionResult CadastraUsuario(Usuario usuario)
        {
            _context.Add(usuario);
            _context.SaveChanges();
            return Created("", usuario);
        }
                                                                                                    

        [HttpPut("{id}")]
        public IActionResult AtualizaUsuario(int id, Usuario usuario) 
        {
            var usuariodoBanco = _context.Usuario.Find(id);
            if (usuariodoBanco == null)
                return NotFound("Usuario não encontrado");
            usuariodoBanco.Email = usuario.Email;
            usuariodoBanco.Nome = usuario.Nome;
            usuariodoBanco.Senha = usuario.Senha;

            _context.SaveChanges();
            return Ok("Atualizado");

        }

        [HttpDelete("{id}")]
        public IActionResult DeletarUsuario(int id)
        {

            var usuariodoBanco = _context.Usuario.Find(id);
            if (usuariodoBanco == null)
                return NotFound("Pessoa não encontrada");
            _context.Remove(usuariodoBanco);
            _context.SaveChanges();
            return Ok("Excluido com sucesso");


        }













    }
}

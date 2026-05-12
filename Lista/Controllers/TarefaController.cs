using Lista.Data;
using Lista.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lista.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {

        private readonly ListaContext _context;

        public TarefaController (ListaContext context)
        {
            _context = context;
        }



        [HttpPost("cadastrar/tarefa")]
        public IActionResult CadastraUsuario(Tarefa tarefa)
        {
            var usuario = HttpContext.Session.GetString("UsuarioLogado");

            if (usuario == null)
                return Unauthorized("Não Autenticado");
            var Id = Request.Cookies["Usuario"];
            if (Id != null)
                tarefa.IdUsuario = int.Parse(Id);

            _context.Add(tarefa);
            _context.SaveChanges();
            return Created("", tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaTarefa(int id, Tarefa tarefa)
        {
            var usuario = HttpContext.Session.GetString("UsuarioLogado");

            if (usuario == null)
                return Unauthorized("Não Autenticado");
            var tarefadoBanco = _context.Tarefa.Find(id);
            if (tarefadoBanco == null)
                return NotFound("Tarefa não encontrado");
            tarefadoBanco.Descricao = tarefa.Descricao;
            tarefadoBanco.Status = tarefa.Status;
            tarefadoBanco.IdUsuario = tarefa.IdUsuario;

            _context.SaveChanges();
            return Ok("Atualizado");

        }

        [HttpDelete("{id}")]
        public IActionResult DeletarTarefa(int id)
        {
            var usuario = HttpContext.Session.GetString("UsuarioLogado");

            if (usuario == null)
                return Unauthorized("Não Autenticado");

            var tarefadoBanco = _context.Tarefa.Find(id);
            if (tarefadoBanco == null)
                return NotFound("Pessoa não encontrada");
            _context.Remove(tarefadoBanco);
            _context.SaveChanges();
            return Ok("Excluido com sucesso");
        }

        [HttpGet("Tarefa/{IdUsuario}")]
        public IActionResult ReservasCliente(int tarefa)
        {
            var resultado = from u in _context.Usuario
                            join t in _context.Tarefa
                            on u.Id equals t.IdUsuario
                            where t.IdUsuario == u.Id
                            select new
                            {
                                Usuario = u.Nome,
                                u.Email,
                                Tarefa = t.Descricao,
                                t.Status,
                                t.IdUsuario
                            };
            return Ok(resultado.ToList());
        }





    }
}

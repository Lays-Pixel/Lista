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



        [HttpPost("cadastrar")]
        public IActionResult CadastraUsuario(Tarefa tarefa)
        {
            var usuariologado = HttpContext.Session.GetString("UsuarioLogado");

            if (usuariologado == null)
                return Unauthorized("Não Autenticado");
            var Id = Request.Cookies["UsuarioLogado"];
            if (Id != null)
                tarefa.IdUsuario = int.Parse(Id);

            _context.Add(tarefa);
            _context.SaveChanges();
            return Created("", tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizaTarefa(int id, Tarefa tarefa)
        {
            var usuariologado = HttpContext.Session.GetString("UsuarioLogado");

            if (usuariologado == null)
                return Unauthorized("Não Autenticado");
            var tarefadoBanco = _context.Tarefas.Find(id);
            if (tarefadoBanco == null)
                return NotFound("Tarefa não encontrado");
            tarefadoBanco.Descricao = tarefa.Descricao;
            tarefadoBanco.Status = tarefa.Status;
           

            _context.SaveChanges();
            return Ok("Atualizado");

        }

        [HttpDelete("{id}")]
        public IActionResult DeletarTarefa(int id)
        {
            var usuariologado = HttpContext.Session.GetString("UsuarioLogado");

            if (usuariologado == null)
                return Unauthorized("Não Autenticado");

            var tarefadoBanco = _context.Tarefas.Find(id);
            if (tarefadoBanco == null)
                return NotFound("Pessoa não encontrada");
            _context.Remove(tarefadoBanco);
            _context.SaveChanges();
            return Ok("Excluido com sucesso");
        }

        [HttpGet]
        public IActionResult BuscarTarefasUsuario()
        {
            var usuariologado = HttpContext.Session.GetString("UsuarioLogado");

            if (usuariologado == null)
                return Unauthorized("Faça login antes!");

            var idUsuarioLogado = Request.Cookies["UsuarioLogado"];
             if(idUsuarioLogado != null)
            {    
                        var Lista = from u in _context.Usuario
                            join t in _context.Tarefas
                            on u.Id equals t.IdUsuario
                            where u.Id == int.Parse(idUsuarioLogado)
                            select new
                            {
                                Usuario = u.Nome, u.Email,
                                Tarefa = t.Descricao, t.Status, t.Id
                            };
                
                return Ok(Lista.ToList());
            }
            return Unauthorized("Faça login antes!");
        }

            



    }
}

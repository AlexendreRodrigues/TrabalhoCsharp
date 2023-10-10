using alexandro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System;
using System.Data;

namespace alexandro.Controllers
{
    [ApiController]
    [Route("api/funcionario")]
    public class FuncionarioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FuncionarioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("cadastrar")]
        public IActionResult Cadastrar(Funcionario funcionario)
        {
            if (funcionario == null || string.IsNullOrEmpty(funcionario.Nome) || string.IsNullOrEmpty(funcionario.Cpf))
            {
                return BadRequest("Dados inválidos.");
            }

            // Verificar se o CPF já existe no banco de dados
            var cpfExistente = _context.funcionario.Any(f => f.Cpf == funcionario.Cpf);
            if (cpfExistente)
            {
                return BadRequest("CPF já cadastrado.");
            }

            _context.funcionario.Add(funcionario);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Cadastrar), new { id = funcionario.FuncionarioId }, funcionario);
        }

        [HttpGet("listar")]
        public IActionResult ListarTodos()
        {
            var funcionarios = _context.funcionario.ToList();
            return Ok(funcionarios);
        }


    }
}

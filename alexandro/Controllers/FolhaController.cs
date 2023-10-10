using Microsoft.EntityFrameworkCore;
using alexandro.Models;
using Microsoft.AspNetCore.Mvc;

namespace alexandro.Controllers
{
    [Route("api/folha")]
    [ApiController]
    public class FolhaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FolhaController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("cadastrar")]
        public IActionResult CadastrarFolha([FromBody] Pagamento entrada)
        {

            var funcionario = _context.funcionario.FirstOrDefault(f => f.FuncionarioId == entrada.FuncionarioId);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }


            var salarioBruto = CalcularSalarioBruto(entrada.Valor, entrada.Quantidade);
            var ir = CalcularIR(salarioBruto);
            var inss = CalcularINSS(salarioBruto);
            var fgts = CalcularFGTS(salarioBruto);
            var salarioLiquido = CalcularSalarioLiquido(salarioBruto, ir, inss);

            var pagamento = new Pagamento
            {
                Valor = entrada.Valor,
                Quantidade = entrada.Quantidade,
                Mes = entrada.Mes,
                Ano = entrada.Ano,
                SalarioBruto = salarioBruto,
                ImpostoIrrf = ir,
                ImpostoInss = inss,
                ImpostoFgts = fgts,
                SalarioLiquido = salarioLiquido,
                FuncionarioId = entrada.FuncionarioId
            };

            _context.pagamentos.Add(pagamento);
            _context.SaveChanges();

            return Ok(pagamento);
        }

        [HttpGet("listar")]
        public IActionResult ListarFolhas()
        {
            var pagamentos = _context.pagamentos
                                     .Include(p => p.Funcionario)
                                     .ToList();
            if (!pagamentos.Any())
            {
                return NotFound("Nenhuma folha de pagamento encontrada.");
            }

            return Ok(pagamentos);
        }

        [HttpPost("buscar/{cpf}/{mes}/{ano}")]
        public IActionResult BuscarFolhaPorCpfMesAno(string cpf, int mes, int ano)
        {

            var funcionario = _context.funcionario.FirstOrDefault(f => f.Cpf == cpf);

            if (funcionario == null)
            {
                return NotFound("Funcionário não encontrado.");
            }

            var pagamento = _context.pagamentos
                                    .FirstOrDefault(p => p.FuncionarioId == funcionario.FuncionarioId &&
                                                         p.Mes == mes &&
                                                         p.Ano == ano);
            if (pagamento == null)
            {
                return NotFound("Folha de pagamento não encontrada.");
            }

            return Ok(pagamento);
        }


        decimal CalcularSalarioBruto(decimal valorHora, int quantidadeHoras)
        {
            return valorHora * quantidadeHoras;
        }

        decimal CalcularIR(decimal salarioBruto)
        {
            if (salarioBruto <= 1903.98M) return 0.0M;
            if (salarioBruto <= 2826.65M) return (salarioBruto * 0.075M) - 142.80M;
            if (salarioBruto <= 3751.05M) return (salarioBruto * 0.15M) - 354.80M;
            if (salarioBruto <= 4664.68M) return (salarioBruto * 0.225M) - 636.13M;
            return (salarioBruto * 0.275M) - 869.36M;
        }

        decimal CalcularINSS(decimal salarioBruto)
        {
            if (salarioBruto <= 1693.72M) return salarioBruto * 0.08M;
            if (salarioBruto <= 2822.90M) return salarioBruto * 0.09M;
            if (salarioBruto <= 5645.80M) return salarioBruto * 0.11M;
            return 621.03M;
        }

        decimal CalcularFGTS(decimal salarioBruto)
        {
            return salarioBruto * 0.08M;
        }

        decimal CalcularSalarioLiquido(decimal salarioBruto, decimal ir, decimal inss)
        {
            return salarioBruto - ir - inss;
        }


    }
}

using System.ComponentModel.DataAnnotations;
namespace alexandro.Models
{
    public class Pagamento
    {
        [Key]
        public int FolhaId { get; set; }
        public decimal Valor { get; set; }
        public int Quantidade { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal ImpostoIrrf { get; set; }
        public decimal ImpostoInss { get; set; }
        public decimal ImpostoFgts { get; set; }
        public decimal SalarioLiquido { get; set; }

        public int FuncionarioId { get; set; }
        public Funcionario? Funcionario { get; set; }
    }
}

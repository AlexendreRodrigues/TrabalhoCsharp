using System.ComponentModel.DataAnnotations;

namespace alexandro.Models
{
    public class Funcionario
    {
        [Key]
        public int FuncionarioId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EmprestimoLivros.Models
{
    public class EmprestimosModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do Recebedor!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Mínimo de 3 caracteres")]
        public String Recebedor { get; set; }

        [Required(ErrorMessage = "Digite o nome do Fornecedor!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Mínimo de 3 caracteres")]
        public string Fornecedor { get; set; }

        [Required(ErrorMessage = "Digite o nome do Livro!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Mínimo de 3 caracteres")]
        public string LivroEmprestado { get; set; }
        public DateTime dataAtualizacao { get; set; }
    }
}

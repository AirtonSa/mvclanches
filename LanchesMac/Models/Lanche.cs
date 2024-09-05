using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanchesMac.Models
{
    [Table("Lanches")] //DIZER QUE É UMA TABELA
    public class Lanche
    { 
        [Key] //KEY SOMENTE PARA CHAVES PRIMARIAS COM ID
        public int LancheId { get; set; }

        [Required(ErrorMessage = "Este Campo é obrigatoriamente preenchido")]
        [Display(Name = "Nome do Lanche")]
        [StringLength(80, MinimumLength = 10, ErrorMessage = "O {0} deve ter no minimo  {1} e no maximo {2}")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Este Campo é obrigatoriamente preenchido")]
        [Display(Name = "Nome do Lanche")]
        [MinLength(20, ErrorMessage = "Descrição deve ter no minimo {1} caractere")] //MINIMO 
        [MaxLength(200,ErrorMessage = "Descrição pode exceder {1} caractere")] //MAXIMO

        public string DescricaoCurta { get; set; }

        [Required(ErrorMessage = "Este Campo é obrigatoriamente preenchido")]
        [Display(Name = "Nome do Lanche")]
        [MinLength(20, ErrorMessage = "Descrição deve ter no minimo {1} caractere")] //MINIMO 
        [MaxLength(200, ErrorMessage = "Descrição pode exceder {1} caractere")] //MAXIMO
        public string DescriçaoDetalhada { get; set; }

        [Required(ErrorMessage = "Informe o preço")]
        [Display(Name = "Preço")]
        [Column(TypeName = "Decimal(10,2)")]
        [Range(1,999.99, ErrorMessage = "O preço deve estar entre 1 e 999,99")]
        public decimal Preco { get; set;}
        [Display(Name = "Caminho imagem normal")]
        [StringLength(80, MinimumLength = 10, ErrorMessage = "O {0} deve ter no minimo  {1} e no maximo {2}")]
        public string ImagemUrl { get; set; }
        [Display(Name = "Caminho imagem normal")]
        [StringLength(80, MinimumLength = 10, ErrorMessage = "O {0} deve ter no minimo  {1} e no maximo {2}")]
        public string ImagemThumbnailUrl { get; set; }

        [Display(Name = "Preferido")]
        public bool IsLanchePreferido { get; set; }

        [Display(Name = "Preferido?")]
        public bool EmEstoque { get; set; }

        [Display(Name = "Categorias")]
        public int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }


    }
}

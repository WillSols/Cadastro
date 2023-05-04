using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cadastro__.Models
{
    public class clienteModel
    {
        //Garantindo que essas paradas tenham valor não nulo saindo do construtor
        public clienteModel()
        {
            inscEstadual = string.Empty;
            genero = "N/A";
            password = string.Empty;
        }

        [DisplayName("Id")]
        public int id { get; set; }

        [DisplayName("Senha")]
        [Required(ErrorMessage = "Por favor, insira uma senha")]
        public string password { get; set; }

        [DisplayName("Nome")]
        [Required]
        public string? name { get; set; }

        [DisplayName("E-mail")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido")]
        [Required]
        public string? email {get; set;}

        [DisplayName("Telefone")]
        [Phone(ErrorMessage = "O telefone informado não é válido")]
        public string? telefone { get; set; }

        [DisplayName("Data de Cadastro")]
        [DataType(DataType.Date)]
        public DateTime? cadData { get; set; }

        [DisplayName("Tipo de Pessoa")]
        public string? tipoPessoa { get; set; }

        [DisplayName("CPF ou CNPJ")]
        [RegularExpression(@"^(\d{3}\.\d{3}\.\d{3}-\d{2}|\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2})$", ErrorMessage = "O CPF/CNPJ informado não é válido")]
        public string? cpfcnpj { get; set; }

        [DisplayName("Inscrição Estadual")]
        [RegularExpression(@"^\d{3}\.\d{5}-\d{2}|ISENTO$", ErrorMessage = "A Inscrição Estadual informada não é válida")]
        public string inscEstadual { get; set;}
        
        [DisplayName("Status do Cadastro")]
        public bool cadStatus { get; set; }

        [DisplayName("Gênero")]
        public string? genero { get; set;}

        [DisplayName("Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime? dataNasc { get; set; }

    }
}

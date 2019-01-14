using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OracleEFCore.UI.Site.Models
{
    public class Empregado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public DateTime DataContratacao { get; set; }
        public decimal Salario { get; set; }
        public int DepartamentoId { get; set; }
    }
}

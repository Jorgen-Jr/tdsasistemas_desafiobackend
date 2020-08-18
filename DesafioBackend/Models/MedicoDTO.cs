using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBackend.Models
{
    public class MedicoDTO
    {
        public Guid id { get; set; }
        public String nome { get; set; }
        public String cpf { get; set; }
        public String crm { get; set; }
        public List<String> especialidades { get; set; }
    }
}

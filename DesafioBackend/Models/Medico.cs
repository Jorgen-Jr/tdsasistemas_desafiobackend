using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DesafioBackend.Models
{
    public class Medico
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required, MaxLength(100)]
        public String nome { get; set; }
        [Required]
        public String cpf { get; set; }
        [Required]
        public String crm { get; set; }
        public ICollection<MedicoEspecialidade> especialidades { get; set; }

    }
}

using Microsoft.EntityFrameworkCore;

namespace DesafioBackend.Models
{
    public class MedicoContext : DbContext
    {
        public MedicoContext(DbContextOptions<MedicoContext> options)
            : base(options)
        { }


        public DbSet<Medico> Medicos { get; set; }
        public DbSet<MedicoEspecialidade> MedicosEspecialidades { get; set; }
        public DbSet<Especialidade> Especialidade { get; set; }

        /* Conexão com o banco de dados PostgreSQL
         * A ser usado váriaveis de ambiente para guardar essa informação.
         */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Host=salt.db.elephantsql.com ;" +
                "Database=mzbjfanh;" +
                "Username=mzbjfanh;" +
                "Password=Nx07XTBv90D-lnvtSksPm8gkSH58Vxmi");
    }
}

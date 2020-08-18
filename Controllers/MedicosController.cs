using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace DesafioBackend.Controllers
{
    [Route("Medico")]
    [ApiController]
    [Authorize]
    public class MedicosController : ControllerBase
    {
        private readonly MedicoContext _context;

        public MedicosController(MedicoContext context)
        {
            _context = context;
        }

        // GET: /Medicos
        [HttpGet]
        public async Task<IEnumerable<MedicoDTO>> GetMedicos()
        {
            var medicos = await _context.Medicos.Include("especialidades.especialidade").ToListAsync();

            List<MedicoDTO> responseMedicos = new List<MedicoDTO>();

            foreach (Medico med in medicos)
            {
                List<String> especialidades = new List<String>();
                foreach (MedicoEspecialidade especialidade in med.especialidades)
                {
                    especialidades.Add(especialidade.especialidade.nome);
                }
                responseMedicos.Add(new MedicoDTO()
                {
                    id = med.id,
                    nome = med.nome,
                    cpf = med.cpf,
                    crm = med.crm,
                    especialidades = especialidades
                });
            }

            return responseMedicos;
        }

        // GET: /Medico/Especialidade
        [HttpGet("{query}")]
        public async Task<IEnumerable<MedicoDTO>> GetMedico(String query)
        {
            var targetEspecialidade = await _context.Especialidade
                .Where(especialidade => especialidade.nome.Contains(query))
                .Include("medicoespecialidade")
                .ToListAsync();

            var medicos = await _context.Medicos
                .Include("especialidades.especialidade")
                .ToListAsync();

            List<MedicoDTO> responseMedicos = new List<MedicoDTO>();

            foreach (Especialidade especialidade in targetEspecialidade)
            {
                foreach (MedicoEspecialidade medicoEspecialidade in especialidade.medicoespecialidade)
                {
                    List<String> especialidades = new List<String>();
                    foreach (MedicoEspecialidade item in medicoEspecialidade.medico.especialidades)
                    {
                        especialidades.Add(item.especialidade.nome);
                    }
                    responseMedicos.Add(new MedicoDTO()
                    {
                        id = medicoEspecialidade.medico.id,
                        nome = medicoEspecialidade.medico.nome,
                        cpf = medicoEspecialidade.medico.cpf,
                        crm = medicoEspecialidade.medico.crm,
                        especialidades = especialidades,

                    });
                }
            }

            return responseMedicos;
        }

        // POST: /Medicos
        [HttpPost]
        public async Task<ObjectResult> PostMedico(MedicoDTO medico)
        {
            List<String> especialidades = new List<String>();

            List<error> ModelErrors = checkfields(medico);

            if (ModelErrors.Count() == 0)
            {
                Medico newMedico = new Medico()
                {
                    nome = medico.nome,
                    cpf = medico.cpf,
                    crm = medico.crm
                };

                foreach (string especialidade in medico.especialidades)
                {
                    Especialidade newEspecialidade = new Especialidade();
                    //A especialidade já existe, basta criar o relacionamento com o médico.
                    if (EspecialidadeExists(especialidade))
                    {
                        var response = _context.Especialidade.Where(item => item.nome == especialidade);

                        newEspecialidade = response.FirstOrDefault();
                    }
                    //A especialidade não existe, basta criar a especialidade e seu relacionamento
                    else
                    {
                        newEspecialidade = new Especialidade()
                        {
                            nome = especialidade
                        };
                    }

                    //_context.Especialidade.Add(newEspecialidade);
                    MedicoEspecialidade newMedicoEspecialidade = new MedicoEspecialidade()
                    {
                        medico = newMedico,
                        especialidade = newEspecialidade
                    };

                    _context.MedicosEspecialidades.Add(newMedicoEspecialidade);
                    newMedico.especialidades.Add(newMedicoEspecialidade);
                }

                _context.Medicos.Add(newMedico);
                await _context.SaveChangesAsync();

                medico.id = newMedico.id;
            }
            else
            {
                return BadRequest(JsonConvert.SerializeObject(new
                {
                    erros = ModelErrors
                }));
            }

            return Ok(medico);
        }


        // DELETE: /Medicos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Medico>> DeleteMedico(Guid id)
        {
            var medico = await _context.Medicos.FindAsync(id);
            if (medico == null)
            {
                return NotFound();
            }

            _context.Medicos.Remove(medico);
            await _context.SaveChangesAsync();

            return medico;
        }

        private bool EspecialidadeExists(String nome)
        {
            return _context.Especialidade.Any(e => e.nome == nome);
        }
        private List<error> checkfields(MedicoDTO medico)
        {
            List<error> formErrors = new List<error>();
            if (medico.especialidades == null || medico.especialidades.Count == 0)
            {
                formErrors.Add(new error()
                {
                    campo = "Especialidades",
                    erro = "Deve ser selecionado ao menos uma especialidade."
                });
            }
            if (String.IsNullOrEmpty(medico.nome))
            {
                formErrors.Add(new error()
                {
                    campo = "Nome",
                    erro = "Nome é obrigatório."
                });
            }
            if (String.IsNullOrEmpty(medico.cpf))
            {
                formErrors.Add(new error()
                {
                    campo = "cpf",
                    erro = "Cpf é obrigatório"
                });
            }
            if (String.IsNullOrEmpty(medico.crm))
            {
                formErrors.Add(new error()
                {
                    campo = "Especialidades",
                    erro = "Crm é obrigatório"
                });
            }
            return formErrors;
        }
    }
    public class error
    {
        public string campo;
        public string erro;
    }
}